using System.Collections.Generic;

namespace ABB.Application.Common
{
    public static class Constant
    {
        public static readonly IDictionary<string, string> ReportMapping = new Dictionary<string, string>()
        {
            {
                "d_uw01r_01_n", "SchedulePolisKebakaran.html"
            },
            {
                "d_uw02r_01_n_db", "CetakanNotaDebetKredit.html"
            },
            {
                "d_uw02r_01_n_kr", "CetakanSlipKomisi.html"
            },
            {
                "d_uw02r_01_n_kw_new", "CetakanKwitansi.html"
            },
            {
                "d_uw01r_01m", "PolisFireMulti.html"
            },
            {
                "d_uw01r_05_n", "PolisMotorSingle.html"
            },
            {
                "d_uw01r_06e", "PolisMotorMulti.html" 
            },
            {
                "d_uw01r_18i", "PolisCargoSingle.html"
            },
            {
                "d_uw01r_18i_m", "PolisCargoMulti.html"
            },
            {
                "d_uw01r_11", "PolisBondingPenawaran.html"
            },
            {
                "d_uw01r_13", "PolisBondingPelaksanaan.html"
            },
            {
                "d_uw01r_15", "PolisBondingUangMuka.html"
            },
            {
                "d_uw01r_16", "PolisBondingPemeliharaan.html"
            },
            {
                "d_uw01r_30", "PolisPABiasaSingle.html"
            },
            {
                "d_uw01r_30m_srm", "PolisPASiramaMulti.html"
            },
            {
                "d_uw01r_34", "PolisHoleinoneSingle.html"
            },
            {
                "d_uw01r_01do", "LampiranPolisFireDaftarIsi.html"
            },
            {
                "d_uw01r_30do_srm2", "LampiranPolisPASiramaObyek.html"
            },
            {
                "d_uw01r_30do", "LampiranPolisPABiasaDaftarisi.html"
            }
        };
        
        public static readonly IDictionary<string, string> ReportStoreProcedureMapping = new Dictionary<string, string>()
        {
            {
                "SchedulePolisKebakaran.html", "spr_uw01r_01"
            },
            {
                "CetakanNotaDebetKredit.html", "spr_uw02r_01_n"
            },
            {
                "CetakanSlipKomisi.html", "spr_uw02r_01_n2"
            },
            {
                "CetakanKwitansi.html", "spr_uw02r_01_n"
            },
            {
                "PolisFireMulti.html", "spr_uw01r_01m"
            },
            {
                "PolisMotorSingle.html", "spr_uw01r_05"
            },
            {
                "PolisMotorMulti.html", "spr_uw01r_05_m"
            },
            {
                "PolisCargoSingle.html", "spr_uw01r_18_eng" 
            },
            {
                "PolisCargoMulti.html", "spr_uw01r_18i_m" 
            },
            {
                "PolisBondingPenawaran.html", "spr_uw01r_11" 
            },
            {
                "PolisBondingPelaksanaan.html", "spr_uw01r_13" 
            },
            {
                "PolisBondingUangMuka.html", "spr_uw01r_15" 
            },
            {
                "PolisBondingPemeliharaan.html", "spr_uw01r_16" 
            },
            {
                "PolisPABiasaSingle.html", "spr_uw01r_30" 
            },
            {
                "PolisPASiramaMulti.html", "spr_uw01r_30m_srm" 
            },
            {
                "PolisHoleinoneSingle.html", "spr_uw01r_34" 
            },
            {
                "LampiranPolisFireDaftarIsi.html", "spr_uw01r_01do" 
            },
            {
                "LampiranPolisPASiramaObyek.html", "spr_uw01r_30do_srm" 
            },
            {
                "LampiranPolisPABiasaDaftarisi.html", "spr_uw01r_30do" 
            }
        };

        public static readonly string HeaderReport = @"<!DOCTYPE html>
                                                        <html lang='id'>
                                                        <head>
                                                            <meta charset='UTF-8'>
                                                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                                            <title>Polis PA Sirama Multi</title>
                                                            <style>
                                                                body {
                                                                    font-family: Arial, sans-serif;
                                                                }

                                                                .container {
                                                                    width: 90%;
                                                                    margin: auto;
                                                                    border: 1px solid black;
                                                                    padding: 25px;
                                                                    page-break-before: always; /* Forces a page break before this element */
                                                                }

                                                                .h1 {
                                                                    text-align: center;
                                                                    font-size: 15px;
                                                                    font-weight: bold;
                                                                    text-transform: uppercase;
                                                                    /* margin-bottom: 15px; */
                                                                }

                                                                .section {
                                                                    margin-top: 10px;
                                                                }

                                                                .table {
                                                                    width: 100%;
                                                                    border-collapse: collapse;
                                                                }

                                                                .table td {
                                                                    padding: 8px;
                                                                    /* border: 1px solid black; */
			                                                        font-size: 12px;
                                                                }
                                                            </style>
                                                        </head>
                                                        <body>";

        public static readonly string FooterReport = @"</body>
                                                        </html>";
    }
}