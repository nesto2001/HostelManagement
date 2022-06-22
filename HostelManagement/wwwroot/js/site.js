// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
<script type="text/javascript">
    $(document).ready(function () {
        $("#ProvinceList").change(function () {
            var selectedProvince = $("#ProvinceList").val();
            $("#DistrictList").empty();
            $("#WardList").empty();
            $.ajax({
                type: "POST",
                url: "/Hostels/Create?handler=LoadDistrict",

                data: selectedProvince,
                contentType: "json; charset=utf-8",

                success: function (districts) {

                    $.each(districts, function (i, district) {
                        $("#DistrictList").append('<option value="' + district.DistrictId + '">' +
                            district.DistrictName + '</option>');
                    });
                },
                error: function (ex) { alert($("#ProvinceList").val() + ex); }
            });
            return false;
        })
    })

        $(function () {
            $("#ProvinceList").on("change", function () {
                var ProvinceId = $(this).val();
                $("#DistrictList").empty();
                $("#DistrictList").append("<option value=''>Select SubCategory</option>");
                $.getJSON(`?handler=LoadDistrict&ProvinceId=${ProvinceId}`, (data) => {
                    $.each(data, function (i, item) {
                        $("#DistrictList").append(`<option value="${item.DistrictId}">${item.DistrictName}</option>`);
                    });
                });
            });
    });
</script>
