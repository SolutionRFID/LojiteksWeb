$(document).ready(function () {
    $('.logos-slider').slick({
        slidesToShow: 6,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 1000,
        arrows: false,
        dots: false,
        pauseOnHover: true,
        responsive: [{
            breakpoint: 768,
            settings: {
                slidesToShow: 1
            }
        }, {
            breakpoint: 1100,
            settings: {
                slidesToShow: 2
            }
        }, {
            breakpoint: 1600,
            settings: {
                slidesToShow: 4
            }
        }]
    });
});
