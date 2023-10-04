$(function() {

  'use strict';

  $('.js-menu-toggle').click(function(e) {

  	var $this = $(this);

  	

  	if ( $('body').hasClass('show-sidebar') ) {
  		$('body').removeClass('show-sidebar');
  		$this.removeClass('active');
  	} else {
  		$('body').addClass('show-sidebar');	
  		$this.addClass('active');
  	}

  	e.preventDefault();

  });

  // click outisde offcanvas
	$(document).mouseup(function(e) {
    var container = $(".sidebar");
    if (!container.is(e.target) && container.has(e.target).length === 0) {
      if ( $('body').hasClass('show-sidebar') ) {
				$('body').removeClass('show-sidebar');
				$('body').find('.js-menu-toggle').removeClass('active');
			}
    }
	}); 

    

});

$(function () {

    $('.btn-link[aria-expanded="true"]').closest('.accordion-item').addClass('active');
    $('.collapse').on('show.bs.collapse', function () {
        $(this).closest('.accordion-item').addClass('active');
    });

    $('.collapse').on('hidden.bs.collapse', function () {
        $(this).closest('.accordion-item').removeClass('active');
    });



});