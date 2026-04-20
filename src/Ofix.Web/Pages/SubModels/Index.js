$(function () {
    var l = abp.localization.getResource('Ofix');
    var createModal = new abp.ModalManager(abp.appPath + 'SubModels/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'SubModels/EditModal');

    var dataTable = $('#SubModelsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(ofix.subModels.subModel.getList),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('Ofix.SubModels.Edit'),
                                action: function (data) {
                                    editModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('Ofix.SubModels.Delete'),
                                action: function (data) {
                                    abp.message.confirm(
                                        l('AreYouSureToDelete', data.record.name),
                                        l('AreYouSure')
                                    ).then(function (confirmed) {
                                        if (confirmed) {
                                            ofix.subModels.subModel
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
                    title: l('Status'),
                    data: "listingStatus",
                    render: function (data) {
                        const map = {
                            0: 'Draft',
                            1: 'Active',
                            2: 'Reserved',
                            3: 'Sold',
                            4: 'Expired',
                            5: 'Passive'
                        };

                        return l('Enum:ListingStatus:' + map[data]);
                    }
                },
                {
                    title: l('Slug'),
                    data: "slug"
                },
                {
                    title: l('CreationTime'),
                    data: "creationTime",
                    dataFormat: "datetime"
                }
            ]
        })
    );

    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'SubModels/CreateModal',
        scriptUrl: abp.appPath + 'Pages/SubModels/CreateModal.js',
        modalClass: 'SubModelCreate'
    });

    createModal.onResult(function () {
        abp.notify.success(l('SuccessfullyCreated'));
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        abp.notify.success(l('SavedSuccessfully'));
        dataTable.ajax.reload();
    });

    $('#NewSubModelButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});