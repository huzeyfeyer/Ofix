$(function () {
    var form = $('#CarListingEditForm');

    var brandSelect = $('#CarListing_BrandId');
    var modelSelect = $('#CarListing_ModelId');
    var subModelSelect = $('#CarListing_SubModelId');
    var mileageInput = $('#CarListing_Mileage');
    var priceInput = $('#CarListing_Price');

    var selectModelText = $('#CarListingLocalization_SelectModel').val();
    var selectSubModelText = $('#CarListingLocalization_SelectSubModel').val();

    function resetSelect($select, placeholder) {
        $select.empty();
        $select.append($('<option>', {
            value: '',
            text: placeholder
        }));
        $select.val('');
    }

    function loadModelsByBrand(brandId) {
        resetSelect(modelSelect, selectModelText);
        resetSelect(subModelSelect, selectSubModelText);

        if (!brandId) {
            return;
        }

        $.ajax({
            url: '/CarListings/Edit?handler=ModelsByBrand&brandId=' + brandId,
            type: 'GET',
            success: function (data) {
                $.each(data, function (index, item) {
                    modelSelect.append($('<option>', {
                        value: item.id,
                        text: item.text
                    }));
                });
            }
        });
    }

    function loadSubModelsByModel(modelId) {
        resetSelect(subModelSelect, selectSubModelText);

        if (!modelId) {
            return;
        }

        $.ajax({
            url: '/CarListings/Edit?handler=SubModelsByModel&modelId=' + modelId,
            type: 'GET',
            success: function (data) {
                $.each(data, function (index, item) {
                    subModelSelect.append($('<option>', {
                        value: item.id,
                        text: item.text
                    }));
                });
            }
        });
    }

    function onlyDigits(value) {
        return (value || '').replace(/\D/g, '');
    }

    function formatThousands(value) {
        var digits = onlyDigits(value);

        if (!digits) {
            return '';
        }

        return digits.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
    }

    function attachNumberFormatting($input) {
        $input.on('focus', function () {
            var digits = onlyDigits($(this).val());
            $(this).val(digits);
        });

        $input.on('blur', function () {
            $(this).val(formatThousands($(this).val()));
        });
    }

    attachNumberFormatting(mileageInput);
    attachNumberFormatting(priceInput);

    brandSelect.on('change', function () {
        loadModelsByBrand($(this).val());
    });

    modelSelect.on('change', function () {
        loadSubModelsByModel($(this).val());
    });

    form.on('submit', function () {
        mileageInput.val(onlyDigits(mileageInput.val()));
        priceInput.val(onlyDigits(priceInput.val()));
    });

    if (mileageInput.val() === '0') {
        mileageInput.val('');
    } else {
        mileageInput.val(formatThousands(mileageInput.val()));
    }

    if (priceInput.val() === '0') {
        priceInput.val('');
    } else {
        priceInput.val(formatThousands(priceInput.val().split(',')[0]));
    }

    if (window.ofix && window.ofix.carListingImages) {
        window.ofix.carListingImages.init();
    }
});