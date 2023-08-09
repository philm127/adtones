//----- Height banner ----- //


/* Script on scroll
------------------------------------------------------------------------------*/
$(window).scroll(function() {
		
	
});

/* Script on resiz
------------------------------------------------------------------------------*/
$(window).resize(function() {
    
});

/* Script on load
------------------------------------------------------------------------------*/
$(window).load(function() {
	//$("video").player.pause();
	
});

/* Script on ready
------------------------------------------------------------------------------*/	
$(document).ready(function(){
	
	
});

/* Script all functions
------------------------------------------------------------------------------*/	
	//---- sticky footer script ----- //
	function StickyFooter(){
		var Stickyfooter =    $('footer').outerHeight()
		$('#wrapper').css('margin-bottom',-Stickyfooter)
		$('#wrapper .footer-push').css('height',Stickyfooter)
	}
	$(window).on("load resize scroll ready",function(){
		var width = $(window).width();
		if( width <= 641){
			$('html,body').css("height","auto");
		}
		else{
			StickyFooter()
			$('html,body').css("height","100%");
		}
	});
