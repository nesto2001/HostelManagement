// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$('.search-input').click(function(){
    $('.input-search').focus();
});

var displayValue = $('.display-capacity').data('value');

$(document).ready(function(){
    $('.display-capacity').html(displayValue+"&nbspadult");
    $('input-capacity').val(displayValue);
})

$('.btn-addnum').click(function(){
    displayValue++;
    $('.display-capacity').html(displayValue+"&nbspadults");
    $('.display-capacity').attr('data-value',displayValue);
    $('.input-capacity').val(displayValue);
})

$('.btn-subnum').click(function(){
    if (displayValue>1)
        displayValue--;
    if (displayValue >= 1){
        $('.display-capacity').html(displayValue+(displayValue == 1? "&nbspadult":"&nbspadults"));
        $('.display-capacity').attr('data-value',displayValue);
        $('.input-capacity').val(displayValue);
    }
})

document.getElementById('ProvinceList').addEventListener('change', (e) => {
    document.getElementById('DistrictList').innerHTML = "<option value=''>Select District</option>";
    fetch(`?handler=LoadDistrict&ProvinceId=${e.target.value}`)
        .then((response) => {
            return response.json();
        })
        .then((data) => {
            Array.prototype.forEach.call(data, function (item, i) {
                $("#DistrictList").append(`<option value="${item.districtId}">${item.districtName}</option>`);
            });
        });
});

document.getElementById('DistrictList').addEventListener('change', (e) => {
    $("#WardList").empty();
    document.getElementById('WardList').innerHTML = "<option value=''>Select Ward</option>";
    fetch(`?handler=LoadWard&DistrictId=${e.target.value}`)
        .then((response) => {
            return response.json();
        })
        .then((data) => {
            Array.prototype.forEach.call(data, function (item, i) {
                $("#WardList").append(`<option value="${item.wardId}">${item.wardName}</option>`);
            });
        });
});


