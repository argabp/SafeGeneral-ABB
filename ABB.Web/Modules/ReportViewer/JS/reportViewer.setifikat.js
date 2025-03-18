$(document).ready(function () {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);

    const kd_cb = urlParams.get('kd_cb');
    const kd_product = urlParams.get('kd_product');
    const kd_thn = urlParams.get('kd_thn');
    const kd_rk = urlParams.get('kd_rk');
    const no_sppa = urlParams.get('no_sppa');
    const no_updt = urlParams.get('no_updt');

    $('#reportView').load("/ReportViewer/LoadSertifikat", { kd_cb: kd_cb, kd_product: kd_product, kd_thn: kd_thn, kd_rk: kd_rk, no_sppa: no_sppa, no_updt: no_updt });
});