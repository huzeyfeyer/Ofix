window.ofix = window.ofix || {};
window.ofix.carListingImages = (function () {
    let pond = null;
    let imageState = [];

    function syncHiddenInput() {
        var hiddenInput = document.getElementById('CarListingImagesState');
        if (hiddenInput) {
            hiddenInput.value = JSON.stringify(imageState);
        }
    }

    function updateStateFromPond() {
        if (!pond) return;

        var files = pond.getFiles();

        var visibleState = files.map((file, index) => {
            var existingId = null;
            var token = null;
            var existingUrl = null;

            if (file.origin === 2) {
                // LOCAL file originally passed in 'files' property
                existingId = file.getMetadata('existingImageId') || null;
                existingUrl = file.source || file.getMetadata('url') || null;
            } else {
                token = file.serverId;
            }

            var oldItem = imageState.find(x => existingId && x.existingImageId === existingId);

            return {
                existingImageId: existingId,
                tempFileToken: token,
                sortOrder: index,
                isCover: index === 0,
                isDeleted: false,
                url: oldItem ? oldItem.url : existingUrl
            };
        });

        var deletedExistingState = imageState
            .filter(old => old.existingImageId && !visibleState.some(v => v.existingImageId === old.existingImageId))
            .map(old => {
                 return {
                     existingImageId: old.existingImageId,
                     tempFileToken: null,
                     sortOrder: 0,
                     isCover: false,
                     isDeleted: true
                 };
            });

        imageState = visibleState.concat(deletedExistingState);

        syncHiddenInput();
    }

    function markDeleted(serverId) {
        if (!serverId) {
            return;
        }

        var existing = imageState.find(function (x) {
            return x.tempFileToken === serverId;
        });

        if (existing) {
            existing.isDeleted = true;
        }

        syncHiddenInput();
    }

    function init() {
        var input = document.getElementById('CarListingPhotos');
        if (!input || typeof FilePond === 'undefined') {
            return;
        }

        FilePond.registerPlugin(
            FilePondPluginImagePreview,
            FilePondPluginFileValidateType,
            FilePondPluginFileValidateSize
        );

        var hiddenInput = document.getElementById('CarListingImagesState');
        var pondFiles = [];
        if (hiddenInput && hiddenInput.value) {
            try {
                var loaded = JSON.parse(hiddenInput.value);
                if (Array.isArray(loaded)) {
                    imageState = loaded;
                    pondFiles = loaded.map(function(item) {
                        return {
                            source: abp.appPath + item.url,
                            options: {
                                type: 'local',
                                file: {
                                    name: item.fileName,
                                    size: item.fileSize,
                                    type: item.contentType || 'image/jpeg'
                                },
                                metadata: {
                                    existingImageId: item.existingImageId,
                                    url: abp.appPath + item.url
                                }
                            }
                        };
                    });
                }
            } catch(e) {}
        }

        pond = FilePond.create(input, {
            name: 'file',
            allowMultiple: true,
            allowReorder: true,
            allowImagePreview: true,
            allowImageExifOrientation: false,
            imagePreviewHeight: 150,
            maxFiles: 10,
            acceptedFileTypes: ['image/jpeg', 'image/png', 'image/webp'],
            maxFileSize: '5MB',
            instantUpload: true,
            files: pondFiles,
            server: {
                load: function (source, load, error, progress, abort, headers) {
                    if (!source) {
                        error('File not found');
                        return;
                    }

                    var request = new XMLHttpRequest();
                    request.open('GET', source);
                    request.responseType = 'blob';
                    request.onload = function() {
                        if (request.status >= 200 && request.status < 300) {
                            var extension = (source.split('.').pop() || 'jpg').split('?')[0];
                            var fileName = 'image.' + extension;
                            var contentType = request.response && request.response.type
                                ? request.response.type
                                : 'image/jpeg';
                            var previewFile = new File([request.response], fileName, { type: contentType });
                            load(previewFile);
                        } else {
                            error('HTTP ' + request.status);
                        }
                    };
                    request.onerror = function() {
                        error('XHR Error');
                    };
                    request.send();
                },
                process: {
                    url: '/CarListings/Create?handler=UploadTemp',
                    method: 'POST',
                    headers: {
                        RequestVerificationToken: abp.security.antiForgery.getToken()
                    },
                    onload: function (response) {
                        try {
                            var parsed = JSON.parse(response);
                            var token = parsed.tempFileToken || parsed.TempFileToken;
                            if (parsed.success !== undefined && parsed.result) {
                                token = parsed.result.tempFileToken || parsed.result.TempFileToken;
                            }
                            return token || response;
                        } catch (e) {
                            return response;
                        }
                    }
                },
                revert: function (uniqueFileId, load, error) {
                    fetch('/CarListings/Create?handler=RemoveTemp&tempFileToken=' + encodeURIComponent(uniqueFileId), {
                        method: 'POST',
                        headers: {
                            RequestVerificationToken: abp.security.antiForgery.getToken()
                        }
                    })
                        .then(function () {
                            markDeleted(uniqueFileId);
                            load();
                        })
                        .catch(function () {
                            error('Revert failed');
                        });
                }
            },
            onupdatefiles: function () {
                updateStateFromPond();
            },
            onreorderfiles: function () {
                updateStateFromPond();
            }
        });

        // Ensure latest state is written just before the form submits
        var form = document.getElementById('CarListingCreateForm') || document.getElementById('CarListingEditForm');
        if (form) {
            form.addEventListener('submit', function () {
                updateStateFromPond();
            });
        }

        syncHiddenInput();
    }

    function addCoverPhotoStyles() {
        var localizationInput = document.getElementById('CarListingLocalization_CoverPhoto');
        var coverHtmlFallback = localizationInput && localizationInput.value ? localizationInput.value : 'Cover Photo';
        var style = document.createElement('style');
        style.type = 'text/css';
        style.innerHTML = `
            .filepond--item:first-child .filepond--file-info-main::after {
                content: " (` + coverHtmlFallback + `)";
                color: #ffcccc;
                font-weight: bold;
                font-size: 0.9em;
                margin-left: 5px;
            }
        `;
        document.head.appendChild(style);
    }

    // Initialize styles right away
    document.addEventListener("DOMContentLoaded", function() {
        addCoverPhotoStyles();
    });

    function getState() {
        return imageState;
    }

    return {
        init: init,
        getState: getState
    };
})();