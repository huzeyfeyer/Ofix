abp.modals.SubModelCreate = function () {
    function initModal(modalManager, args) {
        var $modal = modalManager.getModal();

        var $brand = $modal.find('#BrandId');
        var $model = $modal.find('#SubModel_ModelId');

        var selectBrand = $modal.find('#SubModelLocalization_SelectBrand').val();
        var selectBrandFirst = $modal.find('#SubModelLocalization_SelectBrandFirst').val();
        var selectModel = $modal.find('#SubModelLocalization_SelectModel').val();

        function initBrandSelect() {
            if ($brand.hasClass('select2-hidden-accessible')) {
                $brand.select2('destroy');
            }

            $brand.select2({
                dropdownParent: $modal,
                width: '100%',
                placeholder: selectBrand,
                allowClear: true
            });
        }

        function resetModelDropdown(disabled) {
            if ($model.hasClass('select2-hidden-accessible')) {
                $model.select2('destroy');
            }

            $model.empty();
            $model.append(new Option(selectModel, '', true, false));
            $model.prop('disabled', disabled);

            $model.select2({
                dropdownParent: $modal,
                width: '100%',
                placeholder: disabled ? selectBrandFirst : selectModel,
                allowClear: true
            });
        }

        function loadModelsByBrand(brandId) {
            resetModelDropdown(true);

            if (!brandId) {
                return;
            }

            abp.ajax({
                url: abp.appPath + 'SubModels/CreateModal?handler=ModelsByBrand&brandId=' + brandId,
                type: 'GET'
            }).done(function (result) {
                if ($model.hasClass('select2-hidden-accessible')) {
                    $model.select2('destroy');
                }

                $model.empty();
                $model.append(new Option(selectModel, '', true, false));

                $.each(result, function (index, item) {
                    $model.append(new Option(item.text, item.id, false, false));
                });

                $model.prop('disabled', false);

                $model.select2({
                    dropdownParent: $modal,
                    width: '100%',
                    placeholder: selectModel,
                    allowClear: true
                });
            });
        }

        initBrandSelect();
        resetModelDropdown(true);

        $brand.on('change', function () {
            loadModelsByBrand($(this).val());
        });

        var initialBrandId = $brand.val();
        if (initialBrandId) {
            loadModelsByBrand(initialBrandId);
        }
    }

    return {
        initModal: initModal
    };
};