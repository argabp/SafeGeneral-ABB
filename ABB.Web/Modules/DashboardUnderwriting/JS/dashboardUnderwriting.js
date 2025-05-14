function formatValue(value){
    return value < 0
        ? `(${Math.abs(value).toFixed(2)})`  // If negative: (0.82)
        : value.toFixed(2);
}

$(document).ready(function () {
    var dashboardData = JSON.parse($("#dashboardData").val());
    var produksi = [];
    var target = [];
    var nm_cob = [];
    
    var target_rkap = 0;
    var produksi_sdthnini = 0;
    var produksi_sdthnlalu = 0;
    var produksi_blnini = 0;
    var produksi_blnlalu = 0;
    
    dashboardData.forEach(data => {
        produksi.push((data.produksi_sdthnini / 1000000));
        target.push((data.target_rkap / 1000000));
        nm_cob.push(data.nm_cob.replace(" ", "<br>"));
        target_rkap += data.target_rkap;
        produksi_sdthnini += data.produksi_sdthnini;
        produksi_sdthnlalu += data.produksi_sdthnlalu;
        produksi_blnini += data.produksi_blnini;
        produksi_blnlalu += data.produksi_blnlalu;
    });
    
    $("#target_rkap").text(formatValue(target_rkap));
    $(".produksi_sdthnini").text(formatValue(produksi_sdthnini));
    $("#rasio").text(formatValue(produksi_sdthnini / target_rkap) + "%");
    $("#produksi_sdthnlalu").text(formatValue(produksi_sdthnlalu));
    $("#produksi_naikTurunPerTahun").text(formatValue((produksi_sdthnini - produksi_sdthnlalu) / produksi_sdthnlalu) + "%");
    $("#produksi_blnini").text(formatValue(produksi_blnini));
    $("#produksi_blnlalu").text(formatValue(produksi_blnlalu));
    $("#produksi_naikTurunPerBulan").text(formatValue((produksi_blnini - produksi_blnlalu) / produksi_blnlalu) + "%");

    // ZC.LICENSE = ["569d52cefae586f634c54f86dc99e6a9", "b55b025e438fa8a98e32482b5f768ff5"];

    let chartConfig = {
        type: 'bar3d',
        '3dAspect': {
            depth: 30,
            true3d: 0,
            yAngle: 10,
        },
        backgroundColor: '#fff',
        // title: {
        //     text: 'TARGET DAN REALISASI PRODUKSI UMUM CABANG DAN PEMASARAN S/D TAHUN INI',
        //     fontWeight: 'bold',
        //     height: '40px',
        // },
        legend: {
            backgroundColor: 'none',
            borderColor: 'none',
            item: {
                fontColor: '#333',
            },
            layout: 'float',
            shadow: false,
            width: '90%',
            x: '50%',
            y: '10%',
        },
        plotarea: {
            alpha: 0.3,
            backgroundColor: '#fff',
            margin: '95px 35px 50px 100px',
        },
        plot: {
            valueBox: {
                    thousandsSeparator: ".",
                    decimalsSeparator: ",",
                    offsetY : '-10px'
            },
        },
        scaleX: {
            values: nm_cob,
            alpha: 0.5,
            backgroundColor: '#fff',
            borderColor: '#333',
            borderWidth: '1px',
            guide: {
                visible: false,
            },
            itemOverlap: true,
            item: {
                fontColor: '#333',
                offsetX : '3px',
                fontSize: '8px'
            },
            tick: {
                alpha: 0.2,
                lineColor: '#333',
            },
            shortUnit: "",
            thousandsSeparator: ".",
            decimalsSeparator: ","
        },
        scaleY: {
            label: {
                text: 'Dalam Jutaan Rupiah'
            },
            alpha: 0.5,
            backgroundColor: '#fff',
            borderColor: '#333',
            borderWidth: '1px',
            format: '%v',
            guide: {
                alpha: 0.2,
                lineColor: '#333',
                lineStyle: 'solid',
            },
            item: {
                fontColor: '#333',
                paddingRight: '6px',
            },
            tick: {
                alpha: 0.2,
                lineColor: '#333',
            },
            thousandsSeparator: ".",
            decimalsSeparator: ","
        },
        series: [{
            text: 'Target',
            values: target,
            decimals: 2,
            tooltip: {
                text: 'Target %v',
                backgroundColor: '#03A9F4',
                borderColor: 'none',
                borderRadius: '5px',
                fontSize: '12px',
                padding: '6px 12px',
                shadow: false,
                thousandsSeparator: ".",
                decimalsSeparator: ","
            },
            backgroundColor: '#03A9F4 #4FC3F7',
            borderColor: '#03A9F4',
            legendMarker: {
                borderColor: '#03A9F4',
            },
        },
            {
                text: 'Produksi',
                values: produksi,
                decimals: 2,
                tooltip: {
                    text: 'Produksi %v',
                    backgroundColor: '#673AB7',
                    borderColor: 'none',
                    borderRadius: '5px',
                    fontSize: '12px',
                    padding: '6px 12px',
                    shadow: false,
                    thousandsSeparator: ".",
                    decimalsSeparator: ","
                },
                backgroundColor: '#673AB7 #9575CD',
                borderColor: '#673AB7',
                legendMarker: {
                    borderColor: '#673AB7',
                },
            },
        ],
    };

    zingchart.render({
        id: 'barChart',
        data: chartConfig,
        height: '90%',
        width: '100%',
        defaults: {
            fontFamily: 'sans-serif',
        },
    });
    
    setTimeout(() => {
        $("#barChart-license-text").hide();
    }, 5000);
});
