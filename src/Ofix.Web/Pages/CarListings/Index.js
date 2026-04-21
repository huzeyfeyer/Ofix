$(function () {
    var l = abp.localization.getResource('Ofix');
    var editModal = new abp.ModalManager(abp.appPath + 'CarListings/EditModal');

    var dataTable = $('#CarListingsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(ofix.carListings.carListing.getList),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('Ofix.CarListings.Edit'),
                                action: function (data) {
                                    editModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('Ofix.CarListings.Delete'),
                                action: function (data) {
                                    abp.message.confirm(
                                        l('AreYouSureToDelete', data.record.title),
                                        l('AreYouSure')
                                    ).then(function (confirmed) {
                                        if (confirmed) {
                                            ofix.carListings.carListing
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
                    title: l('Title'),
                    data: "title",
                    render: function (data) {
                        if (!data) {
                            return '';
                        }

                        if (data.length > 35) {
                            return data.substring(0, 35) + '...';
                        }

                        return data;
                    }
                },
                {
                    title: l('SubModel'),
                    data: "subModelName"
                },
                {
                    title: l('Price'),
                    data: "price"
                },
                {
                    title: l('Year'),
                    data: "year"
                },
                {
                    title: l('Mileage'),
                    data: "mileage"
                },
                {
                    title: l('ListingStatus'),
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
                    title: l('Transmission'),
                    data: "transmission",
                    render: function (data) {
                        const map = {
                            0: 'Manual',
                            1: 'Automatic',
                            2: 'SemiAutomatic'
                        };

                        return l('Enum:TransmissionType:' + map[data]);
                    }
                },
                {
                    title: l('FuelType'),
                    data: "fuelType",
                    render: function (data) {
                        const map = {
                            0: 'Petrol',
                            1: 'Diesel',
                            2: 'LPG',
                            3: 'Hybrid',
                            4: 'Electric'
                        };

                        return l('Enum:FuelType:' + map[data]);
                    }
                },
                {
                    title: l('BodyShape'),
                    data: "bodyShape",
                    render: function (data) {
                        const map = {
                            0: 'Sedan',
                            1: 'Hatchback',
                            2: 'SUV',
                            3: 'Coupe',
                            4: 'Wagon'
                        };

                        return l('Enum:BodyShapeType:' + map[data]);
                    }
                },
                {
                    title: l('CreationTime'),
                    data: "creationTime",
                    dataFormat: "datetime"
                }
            ]
        })
    );

  

    editModal.onResult(function () {
        abp.notify.success(l('SavedSuccessfully'));
        dataTable.ajax.reload();
    });

    $('#NewCarListingButton').click(function (e) {
        e.preventDefault();
        window.location.href = abp.appPath + 'CarListings/Create';
    });
});