$(function () {
    if (typeof bootstrap === 'undefined') {
        return;
    }

    var $carousel = $('#CarDetailCarousel');
    if ($carousel.length === 0) {
        return;
    }

    var $counter = $('#CarDetailGalleryCounter');
    var $thumbs = $('#CarDetailGalleryThumbs').find('.car-detail-gallery__thumb');
    var totaal = $carousel.find('.carousel-item').length;

    function zetActieveDuim(positie) {
        $thumbs.removeClass('active');
        var $actief = $thumbs.eq(positie);
        $actief.addClass('active');
        var thumbEl = $actief[0];
        if (thumbEl && typeof thumbEl.scrollIntoView === 'function') {
            thumbEl.scrollIntoView({ inline: 'center', block: 'nearest', behavior: 'smooth' });
        }
    }

    function zetTeller(positie) {
        $counter.text(String(positie + 1) + ' / ' + String(totaal));
    }

    $carousel.on('slid.bs.carousel', function () {
        var positie = $carousel.find('.carousel-item.active').index();
        zetTeller(positie);
        zetActieveDuim(positie);
    });

    $thumbs.on('click', function () {
        var positie = $(this).data('slideTo');
        if (positie === undefined || positie === null) {
            return;
        }
        var instance = bootstrap.Carousel.getOrCreateInstance($carousel[0]);
        instance.to(Number(positie));
    });

    $('#CarDetailGalleryFullscreenBtn').on('click', function () {
        var root = document.getElementById('CarDetailGalleryRoot');
        if (!root) {
            return;
        }
        if (!document.fullscreenElement) {
            root.requestFullscreen().catch(function () { });
        } else {
            document.exitFullscreen().catch(function () { });
        }
    });

    $(document).on('keydown', function (e) {
        var carouselEl = $carousel[0];
        if (!carouselEl) {
            return;
        }
        var instance = bootstrap.Carousel.getOrCreateInstance(carouselEl);

        if (e.key === 'ArrowLeft') {
            e.preventDefault();
            instance.prev();
        } else if (e.key === 'ArrowRight') {
            e.preventDefault();
            instance.next();
        } else if (e.key === 'Escape' && document.fullscreenElement) {
            e.preventDefault();
            document.exitFullscreen().catch(function () { });
        }
    });

    zetTeller(0);
    zetActieveDuim(0);

    var l = abp.localization.getResource('Ofix');

    $('#BodUitbrengenBtn').on('click', function () {
        abp.message.info(l('Marketplace:OfferComingSoon'));
    });

    $('#AfspraakMakenBtn').on('click', function () {
        abp.message.info(l('Marketplace:AppointmentComingSoon'));
    });

    $('.verkoper-contact-btn').on('click', function () {
        abp.message.info(l('Marketplace:MessageComingSoon'));
    });
});
