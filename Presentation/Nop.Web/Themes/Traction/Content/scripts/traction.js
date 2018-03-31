(function () {
    function toggleFooterBlocks(themeBreakpoint) {
        var viewport = sevenSpikes.getViewPort().width;
        var hpfw = $('.home-page-filters-wrapper');

        if (viewport >= themeBreakpoint) {
            $('.footer-block > ul, .news-list-homepage > .news-items').slideDown();

            if (hpfw.length > 0) {
                hpfw.css('margin-top', -(hpfw.innerHeight() + parseInt($('.slider-wrapper').css('margin-bottom'))));
            }
        }
        else {
            $('.footer-block > ul, .news-list-homepage > .news-items').slideUp();

            if (hpfw.length > 0) {
                hpfw.css('margin-top', '');
            }
        }
    }

    $(document).ready(function () {
        var responsiveAppSettings = {
            isEnabled: true,
            themeBreakpoint: 1000,
            isSearchBoxDetachable: true,
            isHeaderLinksWrapperDetachable: true,
            doesDesktopHeaderMenuStick: false,
            doesScrollAfterFiltration: true,
            doesSublistHasIndent: true,
            displayGoToTop: true,
            hasStickyNav: true,
            selectors: {
                menuTitle: ".menu-title span",
                headerMenu: ".header-menu",
                closeMenu: ".close-menu",
                movedElements: ".admin-header-links, .responsive-nav-wrapper, .logo-wrapper, .slider-wrapper, .master-wrapper-content, .footer",
                sublist: ".header-menu .sublist",
                overlayOffCanvas: ".overlayOffCanvas",
                withSubcategories: ".with-subcategories",
                filtersContainer: ".nopAjaxFilters7Spikes",
                filtersOpener: ".filters-button span",
                searchBoxOpener: ".search-wrap > span",
                searchBox: ".search-box.store-search-box",
                searchBoxBefore: ".search-box-reference",
                navWrapper: ".responsive-nav-wrapper",
                navWrapperParent: ".responsive-nav-wrapper-parent",
                headerLinksOpener: "#header-links-opener",
                headerLinksWrapper: ".header-links-wrapper",
                shoppingCartLink: ".shopping-cart-link",
                overlayEffectDelay: 300
            }
        };

        sevenSpikes.initResponsiveTheme(responsiveAppSettings);

        toggleFooterBlocks(responsiveAppSettings.themeBreakpoint);

        sevenSpikes.addWindowEvent("resize", function () { toggleFooterBlocks(responsiveAppSettings.themeBreakpoint); });
        sevenSpikes.addWindowEvent("orientationchange", function () { toggleFooterBlocks(responsiveAppSettings.themeBreakpoint); });

        $('.footer-block > .title, .news-list-homepage > .title').on('click', function () {
            if (sevenSpikes.getViewPort().width < responsiveAppSettings.themeBreakpoint) {
                $(this).siblings('ul, .news-items').slideToggle();
            }
        });

        $(document).on('nivo-slider-finished-loading', function () {
            var hpfw = $('.home-page-filters-wrapper');
            if (sevenSpikes.getViewPort().width > responsiveAppSettings.themeBreakpoint && hpfw.length > 0) {
                hpfw.css('margin-top', -(hpfw.innerHeight() + parseInt($('.slider-wrapper').css('margin-bottom'))));
            }
        });
    });
})();