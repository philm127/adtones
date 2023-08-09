jQuery(document).ready(function($) {
	$("nav a").on('click', function(event) {
		if (this.hash !== "") {
			event.preventDefault();
			var hash = this.hash;
			$('html, body').animate({
				scrollTop: $(hash).offset().top
			}, 800, function() {
				window.location.hash = hash;
			});
		}
	});
	$('#banner-slider').owlCarousel({
		loop: true,
		margin: 10,
		nav: false,
		items: 1,
		dots: true,
		animateOut: 'fadeOut',
		autoplay: true,
		autoplayTimeout: 5000,
		autoplayHoverPause: true,
	});
	$('#content-slider').owlCarousel({
		loop: true,
		margin: 10,
		nav: false,
		items: 1,
		dots: true,
		animateOut: 'fadeOut',
		autoplay: true,
		autoplayTimeout: 5000,
		autoplayHoverPause: true,
	});
	$('#slider').owlCarousel({
		loop: true,
		margin: 0,
		nav: true,
		dots: false,
		autoplay: true,
		autoplayTimeout: 5000,
		autoplayHoverPause: true,
		responsive: {
			0: {
				items: 1
			},
			600: {
				items: 2
			},
			1000: {
				items: 3
			}
		}
	});
});
