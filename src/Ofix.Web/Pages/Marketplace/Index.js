$(function () {
    var l = abp.localization.getResource('Ofix');
    var isActiveFilter = true;
    var currentPage = 1;
    var pageSize = 9;
    var totalCount = 0;

    var $brand = $('#BrandId');
    var $model = $('#ModelId');
    var $subModel = $('#SubModelId');
    var $searchTitle = $('#SearchTitle');
    var $minYear = $('#MinYear');
    var $maxYear = $('#MaxYear');
    var $minPrice = $('#MinPrice');
    var $maxPrice = $('#MaxPrice');
    var $minMileage = $('#MinMileage');
    var $maxMileage = $('#MaxMileage');
    var $fuelType = $('#FuelType');
    var $transmission = $('#Transmission');
    var $bodyShape = $('#BodyShape');
    var $cards = $('#MarketplaceCards');
    var $state = $('#MarketplaceStateMessage');
    var $pageInfo = $('#MarketplacePageInfo');
    var $previousButton = $('#PreviousPageButton');
    var $nextButton = $('#NextPageButton');

    function setStateMessage(message, isVisible) {
        $state.text(message);
        $state.toggleClass('d-none', !isVisible);
    }

    function escapeHtml(value) {
        if (!value) {
            return '';
        }

        return String(value)
            .replaceAll('&', '&amp;')
            .replaceAll('<', '&lt;')
            .replaceAll('>', '&gt;')
            .replaceAll('"', '&quot;')
            .replaceAll("'", '&#039;');
    }

    function zetQueryAlsIngevuld(url, naam, $input) {
        var raw = $input.val();
        if (raw === undefined || raw === null) {
            return;
        }
        var tekst = String(raw).trim();
        if (tekst === '') {
            return;
        }
        url.searchParams.set(naam, tekst);
    }

    function updatePaginationState() {
        var totalPages = Math.max(1, Math.ceil(totalCount / pageSize));
        $pageInfo.text(l('Marketplace:PageInfo', currentPage, totalPages, totalCount));
        $previousButton.prop('disabled', currentPage <= 1);
        $nextButton.prop('disabled', currentPage >= totalPages);
    }

    function loadListings() {
        var cardsUrl = $cards.data('kaarten-url');
        if (!cardsUrl) {
            setStateMessage(l('Marketplace:LoadFailed'), true);
            return;
        }

        setStateMessage(l('Marketplace:Loading'), true);

        var url = new URL(cardsUrl, window.location.href);
        url.searchParams.set('skipCount', String((currentPage - 1) * pageSize));
        url.searchParams.set('maxResultCount', String(pageSize));
        url.searchParams.set('sorting', 'creationTime DESC');

        var title = $searchTitle.val();
        if (title) {
            url.searchParams.set('title', title);
        }

        var brandId = $brand.val();
        if (brandId) {
            url.searchParams.set('brandId', brandId);
        }

        var modelId = $model.val();
        if (modelId) {
            url.searchParams.set('modelId', modelId);
        }

        var subModelId = $subModel.val();
        if (subModelId) {
            url.searchParams.set('subModelId', subModelId);
        }

        zetQueryAlsIngevuld(url, 'minYear', $minYear);
        zetQueryAlsIngevuld(url, 'maxYear', $maxYear);
        zetQueryAlsIngevuld(url, 'minPrice', $minPrice);
        zetQueryAlsIngevuld(url, 'maxPrice', $maxPrice);
        zetQueryAlsIngevuld(url, 'minMileage', $minMileage);
        zetQueryAlsIngevuld(url, 'maxMileage', $maxMileage);
        zetQueryAlsIngevuld(url, 'fuelType', $fuelType);
        zetQueryAlsIngevuld(url, 'transmission', $transmission);
        zetQueryAlsIngevuld(url, 'bodyShape', $bodyShape);

        fetch(url.toString(), {
            credentials: 'same-origin',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        }).then(function (response) {
            var headerTotal = response.headers.get('X-Total-Count');
            if (headerTotal !== null && headerTotal !== '') {
                totalCount = parseInt(headerTotal, 10) || 0;
            }

            if (!response.ok) {
                throw new Error('load failed');
            }

            return response.text();
        }).then(function (html) {
            if (totalCount === 0) {
                $cards.empty();
                setStateMessage(l('Marketplace:NoResults'), true);
                updatePaginationState();
                return;
            }

            setStateMessage('', false);
            $cards.html(html);
            updatePaginationState();
        }).catch(function () {
            setStateMessage(l('Marketplace:LoadFailed'), true);
        });
    }

    function resetSelect($select, placeholderText, disable) {
        $select.empty();
        $select.append('<option value="">' + escapeHtml(placeholderText) + '</option>');
        $select.prop('disabled', disable);
    }

    function loadBrands() {
        return ofix.brands.brand.getList({
            skipCount: 0,
            maxResultCount: 1000,
            sorting: 'name'
        }).then(function (result) {
            resetSelect($brand, l('Marketplace:AllBrands'), false);
            (result.items || []).forEach(function (item) {
                $brand.append('<option value="' + item.id + '">' + escapeHtml(item.name) + '</option>');
            });
        });
    }

    function loadModelsByBrandId(brandId) {
        resetSelect($model, l('Marketplace:AllModels'), !brandId);
        resetSelect($subModel, l('Marketplace:AllSubModels'), true);

        if (!brandId) {
            return Promise.resolve();
        }

        return ofix.models.model.getList({
            skipCount: 0,
            maxResultCount: 1000,
            sorting: 'name',
            brandId: brandId,
            isActive: isActiveFilter
        }).then(function (result) {
            $model.prop('disabled', false);
            (result.items || []).forEach(function (item) {
                $model.append('<option value="' + item.id + '">' + escapeHtml(item.name) + '</option>');
            });
        });
    }

    function loadSubModelsByModelId(modelId) {
        resetSelect($subModel, l('Marketplace:AllSubModels'), !modelId);

        if (!modelId) {
            return Promise.resolve();
        }

        return ofix.subModels.subModel.getList({
            skipCount: 0,
            maxResultCount: 1000,
            sorting: 'name',
            modelId: modelId,
            isActive: isActiveFilter
        }).then(function (result) {
            $subModel.prop('disabled', false);
            (result.items || []).forEach(function (item) {
                $subModel.append('<option value="' + item.id + '">' + escapeHtml(item.name) + '</option>');
            });
        });
    }

    $brand.on('change', function () {
        loadModelsByBrandId($brand.val());
    });

    $model.on('change', function () {
        loadSubModelsByModelId($model.val());
    });

    $('#ApplyFiltersButton').on('click', function () {
        currentPage = 1;
        loadListings();
    });

    $('#ClearFiltersButton').on('click', function () {
        currentPage = 1;
        $searchTitle.val('');
        $brand.val('');
        resetSelect($model, l('Marketplace:AllModels'), true);
        resetSelect($subModel, l('Marketplace:AllSubModels'), true);
        $minYear.val('');
        $maxYear.val('');
        $minPrice.val('');
        $maxPrice.val('');
        $minMileage.val('');
        $maxMileage.val('');
        $fuelType.val('');
        $transmission.val('');
        $bodyShape.val('');
        loadListings();
    });

    $previousButton.on('click', function () {
        if (currentPage <= 1) {
            return;
        }

        currentPage -= 1;
        loadListings();
    });

    $nextButton.on('click', function () {
        var totalPages = Math.ceil(totalCount / pageSize);
        if (currentPage >= totalPages) {
            return;
        }

        currentPage += 1;
        loadListings();
    });

    loadBrands().then(function () {
        loadListings();
    }).catch(function () {
        setStateMessage(l('Marketplace:LoadFailed'), true);
    });
});
