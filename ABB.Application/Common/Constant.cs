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
                "d_uw01r_30m", "PolisPABiasaMulti.html"
            },
            {
                "d_uw01r_30m_srm", "PolisPASiramaMulti.html"
            },
            {
                "d_uw01r_34", "PolisHoleInOneSingle.html"
            },
            {
                "d_uw01r_01do", "LampiranPolisFireDaftarIsi.html"
            },
            {
                "d_uw01r_30do_srm2", "LampiranPolisPASiramaObyek.html"
            },
            {
                "d_uw01r_30do", "LampiranPolisPABiasaDaftarisi.html"
            },
            {
                "d_uw01r_18do_i", "LampiranPolisCargoDaftarisi.html"
            },
            {
                "d_uw01r_27", "LampiranPolisMotorListing.html"
            },
            {
                "d_uw01r_06do", "LampiranPolisMotorDetil.html"
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
                "PolisPABiasaMulti.html", "spr_uw01r_30m" 
            },
            {
                "PolisPASiramaMulti.html", "spr_uw01r_30m_srm" 
            },
            {
                "PolisHoleInOneSingle.html", "spr_uw01r_34" 
            },
            {
                "LampiranPolisFireDaftarIsi.html", "spr_uw01r_01do" 
            },
            {
                "LampiranPolisPASiramaObyek.html", "spr_uw01r_30do_srm" 
            },
            {
                "LampiranPolisPABiasaDaftarisi.html", "spr_uw01r_30do" 
            },
            {
                "LampiranPolisCargoDaftarisi.html", "spr_uw01r_18i_m_do" 
            },
            {
                "LampiranPolisMotorListing.html", "spr_uw01r_27" 
            },
            {
                "LampiranPolisMotorDetil.html", "spr_uw01r_06do" 
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
                                                                    /*font-family: Arial, sans-serif;*/
                                                                    font-size: 11pt;   /* readable in print */
                                                                    line-height: 1.4;  /* adds vertical space */
                                                                }
                                                                td {
                                                                    font-size: 9pt;
                                                                    padding: 2px 4px; /* give breathing room */
                                                                    letter-spacing: 1px;
                                                                }

                                                                .container {
                                                                    border: 1px solid black;
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



                                                                .draft-watermark {
                                                                    position: fixed;
                                                                    top: 50%;
                                                                    left: 50%;
                                                                    font-size: 80px;
                                                                    color: #A9A9A9;
                                                                    opacity: 0.3;
                                                                    z-index: -1;
                                                                    font-weight: bold;
                                                                }
                                                            </style>
                                                        </head>
                                                        <body>";

        public static readonly string FooterReport = @"</body>
                                                        </html>";

        public static readonly string DataDisimpan = "Data Berhasil Disimpan";
        
        public static readonly IDictionary<string, string> AkseptasiObyekViewMapping = new Dictionary<string, string>()
        {
            {
                "d_uw02e_06_01_f", "_ObyekFireFull"
            },
            {
                "d_uw02e_06_04_t", "Empty"
            },
            {
                "d_uw02e_06_02u_t", "Empty"
            },
            {
                "d_uw02e_06_01_t", "Empty"
            },
            {
                "d_uw02e_06_08_f", "_ObyekFire"
            },
            {
                "d_uw02e_06_03_f", "Empty"
            },
            {
                "d_uw02e_06_08_t", "Empty"
            },
            {
                "d_uw02e_06_09_t", "Empty"
            }
        };
        
        public static readonly IDictionary<string, string> AkseptasiOtherViewMapping = new Dictionary<string, string>()
        {
            {
                "d_uw02e_04_01_f", "_OtherFire"
            },
            {
                "d_uw02e_04_02s_f", "_OtherMotor"
            },
            {
                "d_uw02e_04_02_he_f", "_OtherMotor"
            },
            {
                "d_uw02e_04_02_f", "Empty"
            },
            {
                "d_uw02e_04_10_f", "Empty"
            },
            {
                "d_uw02e_04_07_f", "_OtherPA"
            },
            {
                "d_uw02e_04_08_f", "_OtherHoleInOne"
            },
            {
                "d_uw02e_04_03_f", "_OtherCargo"
            },
            {
                "d_uw02e_04_05_f", "_OtherBonding"
            },
            {
                "d_uw02e_04_04_f", "_OtherBonding"
            },
            {
                "d_uw02e_04_06_f", "_OtherHull"
            }
        };
    }
}