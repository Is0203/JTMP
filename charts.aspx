 $.ajax({
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json;',
            url: 'charts.aspx/GetChartValues',
            data: {},
            success:
                function (response) {
                    //generateChartData(response.d);
                    numberData = response.d;
                }

        });
