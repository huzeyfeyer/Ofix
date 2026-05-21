$(function () {
    var l = abp.localization.getResource('Ofix');
    var storageKey = 'ofix-home-last-search';

    var $form = $('#HomeSearchForm');
    var $brand = $('#HomeBrandId');
    var $model = $('#HomeModelId');
    var $minYear = $('#HomeMinYear');
    var $maxPrice = $('#HomeMaxPrice');
    var $bodyShape = $('#HomeBodyShape');
    var $bodyChips = $('.ofix-body-chip');
    var $submit = $('#HomeSearchSubmit');

    var modelsHandler = $form.data('models-handler');
    var countHandler = $form.data('count-handler');
    var countTimer = null;

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

    function resetModelSelect(disabled) {
        $model.empty();
        $model.append('<option value="">' + escapeHtml(l('Marketplace:AllModels')) + '</option>');
        $model.prop('disabled', disabled);
    }

    function loadModels(brandId) {
        resetModelSelect(!brandId);

        if (!brandId || !modelsHandler) {
            return Promise.resolve();
        }

        var separator = modelsHandler.indexOf('?') >= 0 ? '&' : '?';
        return fetch(modelsHandler + separator + 'brandId=' + encodeURIComponent(brandId), {
            credentials: 'same-origin',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('models failed');
                }
                return response.json();
            })
            .then(function (items) {
                $model.prop('disabled', false);
                (items || []).forEach(function (item) {
                    $model.append('<option value="' + item.id + '">' + escapeHtml(item.text) + '</option>');
                });
            });
    }

    function getBodyShapeValue() {
        return $bodyShape.val() || '';
    }

    function setActiveBodyChip(value) {
        $bodyChips.each(function () {
            var $chip = $(this);
            var chipValue = $chip.data('body-shape');
            var isActive = String(chipValue === undefined || chipValue === null ? '' : chipValue) === String(value);
            $chip.toggleClass('active', isActive);
            $chip.attr('aria-pressed', isActive ? 'true' : 'false');
        });
        $bodyShape.val(value);
    }

    function buildCountUrl() {
        var url = new URL(countHandler, window.location.href);
        var brandId = $brand.val();
        var modelId = $model.val();
        var minYear = $minYear.val();
        var maxPrice = $maxPrice.val();
        var bodyShape = getBodyShapeValue();

        if (brandId) {
            url.searchParams.set('brandId', brandId);
        }
        if (modelId) {
            url.searchParams.set('modelId', modelId);
        }
        if (minYear) {
            url.searchParams.set('minYear', minYear);
        }
        if (maxPrice) {
            url.searchParams.set('maxPrice', maxPrice);
        }
        if (bodyShape) {
            url.searchParams.set('bodyShape', bodyShape);
        }

        return url.toString();
    }

    function updateResultCount() {
        if (!countHandler) {
            return;
        }

        fetch(buildCountUrl(), {
            credentials: 'same-origin',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('count failed');
                }
                return response.json();
            })
            .then(function (data) {
                var count = data.count || 0;
                $submit.text(l('Home:SearchResults', count));
            })
            .catch(function () {
                // keep current button text on error
            });
    }

    function scheduleCountUpdate() {
        clearTimeout(countTimer);
        countTimer = setTimeout(updateResultCount, 300);
    }

    function getBodyShapeLabel(value) {
        if (!value && value !== 0) {
            return '';
        }
        var $chip = $bodyChips.filter('[data-body-shape="' + value + '"]');
        return $chip.find('.ofix-body-chip-label').text() || '';
    }

    function saveLastSearch() {
        if (!abp.currentUser.isAuthenticated) {
            return;
        }

        var bodyShapeValue = getBodyShapeValue();
        var payload = {
            brandId: $brand.val() || '',
            modelId: $model.val() || '',
            minYear: $minYear.val() || '',
            maxPrice: $maxPrice.val() || '',
            bodyShape: bodyShapeValue,
            brandText: $brand.find('option:selected').text() || '',
            modelText: $model.find('option:selected').text() || '',
            bodyShapeText: getBodyShapeLabel(bodyShapeValue)
        };

        localStorage.setItem(storageKey, JSON.stringify(payload));
        showLastSearch(payload);
    }

    function buildMarketplaceUrl(filters) {
        var url = new URL('/Marketplace', window.location.origin);

        if (filters.brandId) {
            url.searchParams.set('BrandId', filters.brandId);
        }
        if (filters.modelId) {
            url.searchParams.set('ModelId', filters.modelId);
        }
        if (filters.minYear) {
            url.searchParams.set('minYear', filters.minYear);
        }
        if (filters.maxPrice) {
            url.searchParams.set('maxPrice', filters.maxPrice);
        }
        if (filters.bodyShape) {
            url.searchParams.set('bodyShape', filters.bodyShape);
        }

        return url.toString();
    }

    function showLastSearch(filters) {
        var $section = $('#HomeLastSearchSection');
        if (!$section.length) {
            return;
        }

        var parts = [];
        if (filters.bodyShapeText && filters.bodyShape) {
            parts.push(filters.bodyShapeText);
        }
        if (filters.brandText && filters.brandId) {
            parts.push(filters.brandText);
        }
        if (filters.modelText && filters.modelId) {
            parts.push(filters.modelText);
        }
        if (filters.minYear) {
            parts.push(l('Year') + ': ' + filters.minYear);
        }
        if (filters.maxPrice) {
            parts.push(l('Price') + ': ' + filters.maxPrice);
        }

        if (parts.length === 0) {
            $section.addClass('d-none');
            return;
        }

        $('#HomeLastSearchSummary').text(parts.join(' · '));
        $('#HomeLastSearchLink').attr('href', buildMarketplaceUrl(filters));
        $section.removeClass('d-none');
    }

    function loadLastSearch() {
        if (!abp.currentUser.isAuthenticated) {
            return;
        }

        var raw = localStorage.getItem(storageKey);
        if (!raw) {
            return;
        }

        try {
            var filters = JSON.parse(raw);
            showLastSearch(filters);
        } catch (e) {
            localStorage.removeItem(storageKey);
        }
    }

    $bodyChips.on('click', function () {
        var value = $(this).data('body-shape');
        setActiveBodyChip(value === undefined || value === null ? '' : String(value));
        scheduleCountUpdate();
    });

    $brand.on('change', function () {
        loadModels($brand.val()).then(scheduleCountUpdate);
    });

    $model.on('change', scheduleCountUpdate);
    $minYear.on('input change', scheduleCountUpdate);
    $maxPrice.on('input change', scheduleCountUpdate);

    $form.on('submit', function () {
        if (!getBodyShapeValue()) {
            $bodyShape.prop('disabled', true);
        } else {
            $bodyShape.prop('disabled', false);
        }
        saveLastSearch();
    });

    loadLastSearch();
    scheduleCountUpdate();
});
