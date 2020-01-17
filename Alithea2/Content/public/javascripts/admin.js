$(document).ready(function () {
    $(function () {
        new WOW().init();
    });
    //   $(function() {
    //     TweenMax.staggerFrom($('.tinto'),0.5,{top:100,opacity:0},0.1)
    // });
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });
    $('#sidebarCollapse').on('click', function () {
        $('.register').toggleClass('col-sm-9');
    });
    
    $('#sidebarCollapse').on('click', function () {
        window.setTimeout(function () {
            $('.session').toggleClass('col-sm-12').toggleClass('active-content').attr("right", "0");
            $('.dan-huong').toggleClass('col-sm-11').toggleClass('active-content').attr("right", "0");
        });

    });

    $('.content-message').slideUp();

	// click vao the h3 
	$('.selector').click(function(event) {
		// $('.content-message').slideToggle();
		console.log('Da chay duoc hieu ung click roi ')
		$(this).next().slideToggle();
    });
    $('#getting-started').countdown('2020/02/14', function(event) {
        $(this).html(event.strftime('%w tuần %d ngày %H:%M:%S'));
      });
//slider
    $('#qty_input').prop('disabled', true);
    $('#plus-btn').click(function () {
        $('#qty_input').val(parseInt($('#qty_input').val()) + 1);
    });
    $('#minus-btn').click(function () {
        $('#qty_input').val(parseInt($('#qty_input').val()) - 1);
        if ($('#qty_input').val() == 0) {
            $('#qty_input').val(1);
        }

    });

            
 

var dem = 1;
var  soluonganh = $('.slideanh img').length;

// console.log(soluonganh)

// them 1 the html vao trong vi tri bat ki   
$('.slideanh').append("<img src='' alt='' class='anh'>");
// xu ly vao kich dieu huong
$('.phai').click(function (e) { 
    // bien attr thay doi vi tri
    // x = $('.slideanh img:nth-child(2)').attr('src');
    // $('.slideanh img:nth-child(3)').attr({src:x});
    // console.log(x)

    // xu ly cho boder ben duoi
    var  vitri = $('.boder').index() +1 ;
    $('.anhnho .anh1').removeClass('boder');
    if(vitri == $('.anhnho .anh1').length){
        vitri = 0;
    }
    $('.anhnho .anh1:nth-child('+( vitri+1)+')').addClass('boder');


    // console.log("dem thu xem co bn anh :" + $('.slideanh img').length)
    $('.anh').attr({src:$('.slideanh img:nth-child('+dem+')').attr('src')});

    // console.log('bay gio dem la dem' + dem )
    if(dem == soluonganh )
    {
        dem=0;
        
    }
     dem = dem + 1 ;

});

var dem2 = soluonganh -1;

$('.trai').click(function (e) { 
    //xu li cho phan boder ben duoi
    var  vitri = $('.boder').index() +1 ;
    $('.anhnho .anh1').removeClass('boder');
    if(vitri == 1){
        vitri = $('.anhnho .anh1').length +1;
    }
    $('.anhnho .anh1:nth-child('+( vitri-1)+')').addClass('boder');

    // console.log("dem thu xem co bn anh :" + $('.slideanh img').length)
    $('.anh').attr({src:$('.slideanh img:nth-child('+dem2+')').attr('src')});
    console.log('bay gio dem la dem' + dem2 )
    if( dem2 == 1 )
    {
        dem2 = 4;
        
    }
    dem2 = dem2 - 1 ;
 });

 $('.anhnho .anh1').click(function (e) {  

 console.log( $(this).index() + 1 );
 $('.anh').attr({src:$('.slideanh img:nth-child('+($(this).index() + 1)+')').attr('src')});
$('.anh1').removeClass('boder');
 $(this).addClass('boder');
 });

 
 $('a.next').click(function (e) { 
    
    
    

    $('.owl-wrapper').removeClass('sangtrai');
    $('.owl-wrapper').toggleClass('sangphai')

 });
 $('a.prev').click(function (e) { 
     $('.owl-wrapper').removeClass('sangphai');
    $('.owl-wrapper').addClass('sangtrai');
});

//end slider
 //menu
 jQuery(".ic").bind("mouseenter",function(){          //
    jQuery(this).children("div").slideDown();     //DS con khi hover
});
jQuery(".ic").bind("mouseleave",function(){    
    jQuery(this).children("div").hide()     
});

$('#confirm-delete').on('show.bs.modal', function(e) {
    $(this).find('.btn-ok').attr('href', $(e.relatedTarget).data('href'));
});

//bootstraptoggle

$(function() {
    $('#toggle-one').bootstrapToggle();
  })

    //slider
    $('.nutan .next').click(function (e) {
        $('.sangphai').removeClass('sangphai');
        $('.owl-wrapper').addClass('sangtrai');
    });

    $('.nutan .prev').click(function (e) {
        $('.sangtrai').removeClass('sangtrai');
        $('.owl-wrapper').addClass('sangphai');
    });
    //endslider
    jQuery(window).on('scroll', function (event) {
        // console.log('vi tri');
        // console.log(jQuery('html, body').scrollTop());
        var vitri = jQuery('html, body').scrollTop();
        if (vitri >= 105) {
            jQuery('.nav').addClass('add-menu');
        } else {
            jQuery('.nav').removeClass('add-menu');
        }
    });
    jQuery(window).on('scroll', function (event) {
        // console.log('vi tri');
        // console.log(jQuery('html, body').scrollTop());
        var vitri = jQuery('html, body').scrollTop();
        if (vitri >= 105) {
            jQuery('.block-side').addClass('add-menu1');
        } else {
            jQuery('.block-side').removeClass('add-menu1');
        }
    });

});