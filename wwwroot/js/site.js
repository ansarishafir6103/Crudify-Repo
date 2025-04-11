// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    // jQuery methods go here...
    $('#Employee_CountryId').attr('disabled', false);
    $('#Employee_StateId').attr('disabled', true);
    $('#Employee_CityId').attr('disabled', true);
    if ($('#Employee_CountryId').length > 0 && $('#Employee_CountryId option').length === 0) {
        loadCountries();
    }

    $('#Employee_CountryId').change(function () {
        var countryid = $(this).val();
        if (countryid > 0) {
            loadStates(countryid);
        }
        else {
            alert("select country");//
            $('#Employee_StateId').empty();
            $('#Employee_CityId').empty();
            $('#Employee_StateId').attr('disabled', true);
            $('#Employee_CityId').attr('disabled', true);
            $('#Employee_StateId').append('<option>--select--</option>');
            $('#Employee_CityId').append('<option>--select--</option>');
        }
    });
    $('#Employee_StateId').change(function () {
        var stateid = $(this).val();
        if (stateid > 0) {
            loadCites(stateid); // <-- fix this line
        } else {
            alert("Select state");
            $('#Employee_CityId').empty().append('<option>--select--</option>');
            $('#Employee_CityId').attr('disabled', true);
        }
    });

});
function loadCountries() {
    $('#Employee_CountryId').empty().append('<option>--select--</option>');
    $('#Employee_StateId').empty().append('<option>--select--</option>');
    $('#Employee_CityId').empty().append('<option>--select--</option>');

    $.ajax({
        url: '/Home/GetCountries',
        success: function (response) {
            if (response && response.length > 0) {
                $('#CountryId').attr('disabled', false);

                $.each(response, function (i, data) {
                    if ($("#Employee_CountryId option[value='" + data.countryId + "']").length === 0) {
                        $('#Employee_CountryId').append('<option value="' + data.countryId + '">' + data.countryName + '</option>');
                    }
                });
            } else {
                $('#Employee_CountryId').attr('disabled', true).append('<option>--countries not available--</option>');
                $('#Employee_StateId').attr('disabled', true).append('<option>--states not available--</option>');
                $('#Employee_CityId').attr('disabled', true).append('<option>--cities not available--</option>');
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}
function loadStates(countryid) {
    $('#Employee_StateId').empty().append('<option>--select--</option>');
    $('#Employee_CityId').empty().append('<option>--select--</option>');
    $('#Employee_CityId').attr('disabled', true);

    $.ajax({
        url: '/Home/GetStates?Id=' + countryid,
        success: function (response) {
            if (response && response.length > 0) {
                $('#Employee_StateId').attr('disabled', false);

                $.each(response, function (i, data) {
                    if ($("#Employee_StateId option[value='" + data.stateId + "']").length === 0) {
                        $('#Employee_StateId').append('<option value="' + data.stateId + '">' + data.stateName + '</option>');
                    }
                });
            } else {
                $('#Employee_StateId').attr('disabled', true).append('<option>--states not available--</option>');
                $('#Employee_CityId').attr('disabled', true).append('<option>--cities not available--</option>');
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}
function loadCites(statesid) {
    $('#Employee_CityId').empty().append('<option>--select--</option>');
    $('#Employee_CityId').attr('disabled', true); // Reset it before call

    $.ajax({
        url: '/Home/GetCities?Id=' + statesid,
        success: function (response) {
            if (response && response.length > 0) {
                $('#Employee_CityId').attr('disabled', false); // ✅ Enable it here

                $.each(response, function (i, data) {
                    if ($("#Employee_CityId option[value='" + data.cityId + "']").length === 0) {
                        $('#Employee_CityId').append('<option value="' + data.cityId + '">' + data.cityName + '</option>');
                    }
                });
            } else {
                $('#Employee_CityId').attr('disabled', true);
                $('#Employee_CityId').append('<option>--cities not available--</option>');
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}
