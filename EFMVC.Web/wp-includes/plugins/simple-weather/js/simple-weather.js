if(typeof SimpleWeather.locale !== 'undefined' ){
    moment.defineLocale('en-SimpleWeather', SimpleWeather.locale);
    moment.locale('en-SimpleWeather');
}
Vue.filter('momentjs', function (date, format, convert) {
    var offset = SimpleWeather.locale.gmtOffset;
    var sign = offset.toString().indexOf('-') === 0 ? '-' : '+';
    hours = parseInt( offset / 3600 );
    hours = hours < 10 ? '0' + hours : hours;
    minutes = offset % 3600;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var utc = sign + hours + ':' + minutes;
    moment.defineLocale('en-SimpleWeather', SimpleWeather.locale);
    moment.locale('en-SimpleWeather');
    return convert !== false ? moment(date * 1000).utcOffset(utc).format(format) : moment(date * 1000).format(format);
});

var simple_weather_apps = [];

Array.prototype.forEach.call( document.querySelectorAll('div.simple-weather--vue'), function(el, index){

    var id = el.getAttribute('id').replace( 'simple-weather--', '' );

    simple_weather_apps[index] = new Vue({
        el: '#simple-weather--' + id,
        mounted: function() {
            var vm = this;
            var classes = [ 'simple-weather--vue-mounted' ];

            if( typeof vm.atts.text_align !== 'undefined' && vm.atts.text_align !== null && vm.atts.text_align.length > 0 ){
                classes.push( 'simple-weather--text-' + vm.atts.text_align );
            }

            if( typeof vm.atts.style !== 'undefined' && vm.atts.style !== null && vm.atts.style.length > 0 ){
                classes.push( 'simple-weather--view-' + vm.atts.style );
            }

            if( typeof vm.atts.display !== 'undefined' &&  vm.atts.display === 'block' ){
                classes.push( 'simple-weather--display-block' );
            }

            if( typeof vm.show_date !== 'undefined' && ! vm.filter_var( vm.atts.show_date ) ){
                classes.push( 'simple-weather--hidden-date' );
            }

            vm.$el.className += ' ' + classes.join( ' ' );

            if(typeof vm.feed !== 'undefined' && Object.keys(vm.feed).length === 0 && vm.feed.constructor === Object ){
                vm.get_weather();
            }
        },
        data: function(){

            var vm = this;
            var error = '&nbsp;';
            var atts = typeof window['SimpleWeatherAtts'] !== 'undefined' && typeof window['SimpleWeatherAtts'][id] !== 'undefined' ? window['SimpleWeatherAtts'][id] : {};
            var feed = {};
            var feed_current_weather = {};

            if( typeof atts.station !== 'undefined' && atts.station === 'darksky' ){
                if( typeof window['SimpleWeatherFeeds'] !== 'undefined' && typeof window['SimpleWeatherFeeds'][id] !== 'undefined' && typeof window['SimpleWeatherFeeds'][id].forecast !== 'undefined' ){
                    feed = window['SimpleWeatherFeeds'][id].forecast.daily;
                    feed_current_weather = typeof window['SimpleWeatherFeeds'][id].forecast.currently !== 'undefined' ? window['SimpleWeatherFeeds'][id].forecast.currently : {};
                } else if( typeof window['SimpleWeatherFeeds'] !== 'undefined' && typeof window['SimpleWeatherFeeds'][id] !== 'undefined' && typeof window['SimpleWeatherFeeds'][id].errors !== 'undefined' ){
                    for( var i in window['SimpleWeatherFeeds'][id].errors ){
                        if (! window['SimpleWeatherFeeds'][id].errors.hasOwnProperty(i)) continue;
                        if( [ 'could_not_get_location', 'no_api' ].indexOf( i ) >= 0 ) error = window['SimpleWeatherFeeds'][id].errors[i][0];
                    }
                }
            } else {
                if( typeof window['SimpleWeatherFeeds'] !== 'undefined' && typeof window['SimpleWeatherFeeds'][id] !== 'undefined' ){
                    feed = window['SimpleWeatherFeeds'][id].forecast;
                    feed_current_weather = typeof window['SimpleWeatherFeeds'][id].current !== 'undefined' ? window['SimpleWeatherFeeds'][id].current : {};
                }
            }

            return {
                atts: atts,
                feed: feed,
                feed_current_weather: feed_current_weather,
                visible: [],
                error: error
            }
        },
        filters: {
            temp: function(temp){
                return typeof temp !== 'undefined' ? parseInt( temp ) : '';
            }
        },
        computed: {
            units: function(){
                return typeof this.atts.units !== 'undefined' && this.atts.units === 'imperial' ? 'F' : 'C';
            },
            units_wind: function(){
                return typeof this.atts.units !== 'undefined' && this.atts.units === 'imperial' ? 'mph' : 'kph';
            },
            current_weather: function(){
                var vm = this;
                var out = {};

                if( vm.weather_station === 'openweather' ){
                    if(typeof vm.feed_current_weather.weather !== 'undefined' && typeof vm.feed_current_weather.weather[0].id !== 'undefined' ){
                        out.id = vm.feed_current_weather.weather[0].id;
                        out.desc = vm.feed_current_weather.weather[0].description;
                    }
                    if(typeof vm.feed_current_weather.dt !== 'undefined' ) out.dt = vm.feed_current_weather.dt;
                    if(typeof vm.feed_current_weather.main !== 'undefined' && typeof vm.feed_current_weather.main.temp !== 'undefined' ){
                        out.temp = vm.feed_current_weather.main.temp;
                        out.humidity = Math.ceil( vm.feed_current_weather.main.humidity );
                    }
                    if(typeof vm.feed_current_weather.clouds !== 'undefined' ) out.clouds = Math.ceil( vm.feed_current_weather.clouds.all );
                    if(typeof vm.feed_current_weather.wind !== 'undefined' ) {
                        out.wind = {};
                        out.wind.deg = vm.getWindDirection( vm.feed_current_weather.wind.deg );
                        out.wind.speed = vm.units === 'metric' ? vm.feed_current_weather.wind.speed * 3.6 : vm.feed_current_weather.wind.speed * 3.6 / 1.609344;
                        out.wind.speed = Math.ceil(out.wind.speed);
                    }
                    if(typeof vm.feed_current_weather.name !== 'undefined' ) out.name = vm.feed_current_weather.name;
                }

                if( vm.weather_station === 'darksky' ){
                    if(typeof vm.feed_current_weather.icon !== 'undefined' ) out.id = vm.feed_current_weather.icon;
                    if(typeof vm.feed_current_weather.summary !== 'undefined' ) out.desc = vm.feed_current_weather.summary;
                    if(typeof vm.feed_current_weather.humidity !== 'undefined' ) out.humidity = Math.ceil( vm.feed_current_weather.humidity * 100 );
                    if(typeof vm.feed_current_weather.cloudCover !== 'undefined' ) out.clouds = Math.ceil( vm.feed_current_weather.cloudCover * 100 );

                    if(typeof vm.feed_current_weather.windBearing !== 'undefined' && typeof vm.feed_current_weather.windSpeed !== 'undefined' ) {
                        out.wind = {};
                        if(typeof vm.feed_current_weather.windBearing !== 'undefined' ) out.wind.deg = vm.getWindDirection( vm.feed_current_weather.windBearing );
                        if(typeof vm.feed_current_weather.windSpeed !== 'undefined' ) out.wind.speed = vm.feed_current_weather.windSpeed;
                    }

                    if(typeof vm.feed_current_weather.time !== 'undefined' ) out.dt = vm.feed_current_weather.time;
                    if(typeof vm.feed_current_weather.temperature !== 'undefined' ) out.temp = vm.feed_current_weather.temperature;
                }
                return out;
            },
            weather_feed: function(){
                var vm = this;
                var out = [];
                var dt = moment().utcOffset(this.get_utc_offset());
                var days = {};

                if( vm.weather_station === 'openweather' ){
                    if( typeof vm.feed !== 'undefined' && typeof vm.feed.list !== 'undefined' ){

                        vm.feed.list.forEach(function(val, index){

                            var dayname = val.dt_txt.split( ' ' );
                            dayname = dayname[0].replace('-', '_');

                            if( typeof days[dayname] !== 'undefined' ){
                                days[dayname].push( val.main.temp );
                            } else {
                                days[dayname] = [ val.main.temp ];
                            }
                        });

                        vm.feed.list.forEach(function(val, index){

                            var dayname = val.dt_txt.split( ' ' );
                            dayname = dayname[0].replace('-', '_');

                            if( ! dt.isSame( val.dt * 1000, 'day' ) ) {

                                var max = typeof days[dayname] !== 'undefined' ? days[dayname].reduce(function(a, b) {
                                    return Math.max(a, b);
                                }) : null;

                                var min = typeof days[dayname] !== 'undefined' ? days[dayname].reduce(function(a, b) {
                                    return Math.min(a, b);
                                }) : null;

                                out.push({
                                    dt: val.dt,
                                    id: val.weather[0].id,
                                    temp: max,
                                    temp_min: min
                                });

                            }

                            dt = moment( val.dt * 1000 ).utcOffset( vm.get_utc_offset() );

                        });
                    }

                }

                if( vm.weather_station === 'darksky' ){
                    if( typeof vm.feed !== 'undefined' && typeof vm.feed.data !== 'undefined' ){
                        vm.feed.data.forEach(function(val, index){

                            if( ! dt.isSame( val.time * 1000, 'day' ) && dt.isBefore( val.time * 1000, 'day' )  ){
                                out.push({
                                    dt: val.time,
                                    id: val.icon,
                                    temp: val.apparentTemperatureMax,
                                    temp_min: val.apparentTemperatureMin
                                });
                            }
                            last = moment(val.time * 1000).utcOffset( vm.get_utc_offset() ).format('YYYY/MM/DD');
                            dt = moment(val.time * 1000).utcOffset( vm.get_utc_offset() );
                        });
                    }
                }
                return out.length > 0 ? out : false;
            },
            weather_station : function(){
                var vm = this;
                return typeof vm.atts.station !== 'undefined' && vm.atts.station === 'darksky' ? 'darksky' : 'openweather';
            },
            style: function(){
                var vm = this;
                if( typeof vm.atts.style === 'undefined' ) return 'inline';
                return vm.atts.style;
            }
        },
        methods: {
            in_darkSky_feed: function(){
                var vm = this;
            },
            get_utc_offset: function(){
                var vm = this;
                var offset = SimpleWeather.locale.gmtOffset;
                var sign = offset.toString().indexOf('-') === 0 ? '-' : '+';
                hours = parseInt( offset / 3600 );
                hours = hours < 10 ? '0' + hours : hours;
                minutes = offset % 3600;
                minutes = minutes < 10 ? '0' + minutes : minutes;
                var utc = sign + hours + ':' + minutes;
                return utc;
            },
            getWindDirection: function($deg){
                if ($deg >= 0 && $deg < 22.5) return 'N';
                else if ($deg >= 22.5 && $deg < 45) return 'NNE';
                else if ($deg >= 45 && $deg < 67.5) return 'NE';
                else if ($deg >= 67.5 && $deg < 90) return 'ENE';
                else if ($deg >= 90 && $deg < 122.5) return 'E';
                else if ($deg >= 112.5 && $deg < 135) return 'ESE';
                else if ($deg >= 135 && $deg < 157.5) return 'SE';
                else if ($deg >= 157.5 && $deg < 180) return 'SSE';
                else if ($deg >= 180 && $deg < 202.5) return 'S';
                else if ($deg >= 202.5 && $deg < 225) return 'SSW';
                else if ($deg >= 225 && $deg < 247.5) return 'SW';
                else if ($deg >= 247.5 && $deg < 270) return 'WSW';
                else if ($deg >= 270 && $deg < 292.5) return 'W';
                else if ($deg >= 292.5 && $deg < 315) return 'WNW';
                else if ($deg >= 315 && $deg < 337.5) return 'NW';
                else if ($deg >= 337.5 && $deg < 360) return 'NNW';
                else return;
            },
            getWeatherIcon: function( day ){
                return this.weather_station === 'darksky' ? 'sw-forecast-io-' + day.id : 'sw-owm-' + day.id;
            },
            isset: function( $key ){
                return typeof vm.atts[$key] !== 'undefined' &&  vm.atts[$key] !== null &&  vm.atts[$key].length > 0;
            },
            hasCurrentWeather: function(){
                var vm = this;
                return vm.filter_var( vm.atts.show_current ) && Object.keys(vm.current_weather).length > 0;
            },
            isDayVisible: function(index, day){
                var vm = this;
                var out = false;
                var days = parseInt( vm.atts.days );
                if( vm.filter_var( vm.atts.show_current ) && moment( vm.current_weather.dt * 1000 ).utcOffset(this.get_utc_offset()).isSame( day.dt * 1000, 'day' ) ) return false;
                return index < days;
            },
            filter_var: function( test ){
                return [1, true, 'on', 'true', '1', 'yes'].indexOf(test) >= 0 ? true : false;
            },
            get_weather: function(){
                var vm = this;
                vm.$http.post(
                    window.SimpleWeather.rest_route + 'simple-weather/v1/get_weather/',
                    vm.atts,
                    {
                        emulateJSON: true
                    }
                ).then(vm.successCallback, vm.errorCallback);
            },
            successCallback: function(response){
                var vm = this;
                if( vm.filter_var( window.SimpleWeather.settings.console_log ) ) console.log(response);
                if( vm.weather_station === 'openweather' ){
                    if( response.status >= 200 && response.status <= 202 ){
                        if( typeof response.body.current !== 'undefined' ){
                            vm.feed_current_weather = response.body.current;
                        }
                        if( typeof response.body.forecast !== 'undefined' ){
                            vm.feed = response.body.forecast;
                        }
                    }
                }

                if( vm.weather_station === 'darksky' ){
                    if( response.status >= 200 && response.status <= 202 ){
                        if( typeof response.body.forecast !== 'undefined' &&  typeof response.body.forecast.currently !== 'undefined' ){
                            vm.feed_current_weather = response.body.forecast.currently;
                        }
                        if( typeof response.body.forecast && typeof response.body.forecast.daily !== 'undefined' ){
                            vm.feed = response.body.forecast.daily;
                        }
                    }
                }

            },
            errorCallback: function(response){
                var vm = this;
                if( vm.filter_var( window.SimpleWeather.settings.console_log ) ) console.log(response);
                if( typeof response.body.code !== 'undefined' && [ 'could_not_get_location', 'no_api' ].indexOf( response.body.code ) >= 0 ){
                    vm.error = response.body.message;
                }
            }
        }
    });

});
