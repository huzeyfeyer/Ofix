$(function () {
    var l = abp.localization.getResource('Ofix');

    var createModal = new abp.ModalManager(abp.appPath + 'Brands/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Brands/EditModal');
    var currentEditBrandId = null;

    async function uploadLogoWithFetch(id, file) {
        const formData = new FormData();
        formData.append("file", file);

        const response = await fetch(`/api/app/brand/${id}/upload-logo`, {
            method: "POST",
            body: formData,
            headers: {
                RequestVerificationToken: abp.security.antiForgery.getToken()
            }
        });

        const responseText = await response.text();

        if (!response.ok) {
            throw new Error(responseText || 'Logo upload failed.');
        }
    }

    var dataTable = $('#BrandsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(ofix.brands.brand.getList),
            columnDefs: [
                {
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                action: function (data) {
                                    editModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                action: function (data) {
                                    abp.message.confirm(
                                        l('AreYouSureToDelete', data.record.name),
                                        l('AreYouSure')
                                    ).then(function (confirmed) {
                                        if (confirmed) {
                                            ofix.brands.brand
                                                .delete(data.record.id)
                                                .then(function () {
                                                    abp.notify.success(l('SuccessfullyDeleted'));
                                                    dataTable.ajax.reload();
                                                });
                                        }
                                    });
                                }
                            }
                        ]
                    }
                },
                {
                    title: l('Name'),
                    data: "name"
                },
                {
                    title: l('OrderNo'),
                    data: "orderNo"
                },
                {
                    title: l('IsActive'),
                    data: "isActive",
                    render: function (data) {
                        return data
                            ? '<span class="badge bg-success">' + l('Active') + '</span>'
                            : '<span class="badge bg-secondary">' + l('Passive') + '</span>';
                    }
                },
                {
                    title: l('Slug'),
                    data: "slug"
                },
                {
                    title: l('Logo'),
                    data: null,
                    orderable: false,
                    render: function (data, type, row) {
                        if (!row.logoBlobName) {
                            return '<span>No Logo</span>';
                        }

                        var url = `/api/app/brand/${row.id}/logo`;
                        return `<img src="${url}" style="max-height:40px;" onerror="this.outerHTML='<span>No Logo</span>'" />`;
                    }
                },
                {
                    title: l('CreationTime'),
                    data: "creationTime",
                    render: function (data) {
                        return luxon.DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            })
                            .toLocaleString(luxon.DateTime.DATETIME_SHORT);
                    }
                }
            ]
        })
    );

    createModal.onResult(async function (data) {
        var fileInput = document.querySelector('#CreateLogoFile');
        var file = fileInput?.files[0];

        if (!file) {
            dataTable.ajax.reload();
            return;
        }

        try {
            await uploadLogoWithFetch(data.id, file);
            dataTable.ajax.reload();
        } catch (err) {
            abp.message.error(err.message || 'Logo upload failed.');
        }
    });

    editModal.onResult(async function () {
        var fileInput = document.querySelector('#EditLogoFile');
        var file = fileInput?.files[0];

        if (!file) {
            dataTable.ajax.reload();
            return;
        }

        try {
            await uploadLogoWithFetch(currentEditBrandId, file);
            dataTable.ajax.reload();
        } catch (err) {
            abp.message.error(err.message || 'Logo upload failed.');
        }
    });

    editModal.onOpen(function () {
        setTimeout(function () {
            var id = $('input[name="Id"]').val();
            currentEditBrandId = id;

            var img = document.getElementById("CurrentLogoPreview");
            var noLogo = document.getElementById("NoLogoText");

            if (!id || !img || !noLogo) {
                return;
            }

            var url = `/api/app/brand/${id}/logo`;

            img.onload = function () {
                img.style.display = "block";
                noLogo.style.display = "none";
            };

            img.onerror = function () {
                img.style.display = "none";
                noLogo.style.display = "block";
            };

            img.src = url;
        }, 300);
    });

    $('#NewBrandButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});