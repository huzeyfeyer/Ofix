$(function () {
    var l = abp.localization.getResource('Ofix');
    var listingStatusActive = 1;
    var currentPage = 1;
    var pageSize = 9;
    var totalCount = 0;

    var $brand = $('#BrandId');
    var $model = $('#ModelId');
    var $subModel = $('#SubModelId');
    var $searchTitle = $('#SearchTitle');
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

    function getFuelTypeLabel(fuelType) {
        var map = {
            0: 'Petrol',
            1: 'Diesel',
            2: 'LPG',
            3: 'Hybrid',
            4: 'Electric'
        };

        if (map[fuelType] === undefined) {
            return '';
        }

        return l('Enum:FuelType:' + map[fuelType]);
    }

    function renderCards(items) {
        if (!items || items.length === 0) {
            $cards.empty();
            setStateMessage(l('Marketplace:NoResults'), true);
            return;
        }

        setStateMessage('', false);

        var cardsHtml = items.map(function (item) {
            var fuelTypeText = getFuelTypeLabel(item.fuelType);
            var isNewBadge = item.listingStatus === listingStatusActive
                ? '<span class="badge rounded-pill bg-warning text-dark mb-3">' + escapeHtml(l('VehicleCard:New')) + '</span>'
                : '';

            return '' +
                '<div class="col-12 col-md-6 col-xl-4">' +
                '  <article class="vehicle-card h-100 border rounded-3 overflow-hidden bg-white">' +
                '    <a href="#" class="text-decoration-none text-reset d-block">' +
                '      <div class="vehicle-card-image-wrapper">' +
                '        <img src="' + escapeHtml(item.coverImageUrl || 'https://placehold.co/640x360?text=Ofix') + '" alt="' + escapeHtml(item.title) + '" class="vehicle-card-image w-100" />' +
                '      </div>' +
                '      <div class="vehicle-card-content p-3">' +
                '        <h3 class="h5 mb-2 fw-bold">' + escapeHtml(item.title || '') + '</h3>' +
                '        <div class="fs-4 fw-bold mb-2">€ ' + escapeHtml(item.price) + '</div>' +
                '        <div class="text-muted mb-3">' + escapeHtml(item.year) + ' | ' + escapeHtml(item.mileage) + ' km | ' + escapeHtml(fuelTypeText) + '</div>' +
                '        ' + isNewBadge +
                '      </div>' +
                '    </a>' +
                '  </article>' +
                '</div>';
        }).join('');

        $cards.html(cardsHtml);
    }

    function updatePaginationState() {
        var totalPages = Math.max(1, Math.ceil(totalCount / pageSize));
        $pageInfo.text(l('Marketplace:PageInfo', currentPage, totalPages, totalCount));
        $previousButton.prop('disabled', currentPage <= 1);
        $nextButton.prop('disabled', currentPage >= totalPages);
    }

    function loadListings() {
        setStateMessage(l('Marketplace:Loading'), true);

        ofix.carListings.carListing.getList({
            skipCount: (currentPage - 1) * pageSize,
            maxResultCount: pageSize,
            sorting: 'creationTime DESC',
            title: $searchTitle.val() || null,
            subModelId: $subModel.val() || null,
            listingStatus: listingStatusActive
        }).then(function (result) {
            totalCount = result.totalCount || 0;
            renderCards(result.items || []);
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
            listingStatus: listingStatusActive
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
            listingStatus: listingStatusActive
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
