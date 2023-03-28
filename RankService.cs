using CsGOStateEmitter.Helper;
using System.Reflection;

namespace CsGOStateEmitter
{
    public class RankService
    {

        public static string GetFileLayout(string path, string nameFile)
        {
            //var rootPath = ConfigHelper.Get<string>($"Pathwwwroot");
            //if (string.IsNullOrWhiteSpace(rootPath) || string.IsNullOrWhiteSpace(nameFile) || string.IsNullOrWhiteSpace(path))
            //    return string.Empty;

            //string applicationPath = Path.Combine(Directory.GetCurrentDirectory(), @$"{rootPath}");

            var pathCurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

#if DEBUG
            pathCurrentDirectory = Directory.GetCurrentDirectory();
#endif

            return Path.Combine(Path.Combine(pathCurrentDirectory, path), nameFile);
        }

        public static string GetRank(int points)
        {
            if (points > 0 && points <= 500)
                return "https://uca70141387dcf4d59168c8458d6.previews.dropboxusercontent.com/p/thumb/AB3BKPUwUgztjmFprCZ2wHKYKpmL-t1GsKiw55rlU43rB8n7wrmfTFOi-ON2VecqjkHl1U2AkXn0yj0vJq9-FxeY_9z3SUUN7-T0sdRBe-jc8xLB6iVuB9rdpKu05T2WWKiAwB8u7Xl8pWU1-iTPNvfarGzGbTPllv0x0G9Yav_x7K_HffbfbLH5bENpNV-hpMJvJl4wIoW2n5Su_PKNfE3X9ZMez6wCh7j2ivwjEZs9vLXDM71XO8u243PqEzZcBFQzPhtuBo01Ad9gX_AQosSm4SBP1wtst7QeajLQNalGwYnVLdm2XoSZBzSQX75WkVeC6ddH_nqyldE59psb9yy1cbpOPTAJt_ouyXz0Hp1rIv3Vxbm5QPp-E58X21L9Wy0/p.png";
            if (points > 500)
                return "https://uc6a37bcee6fbfd64f373885ff37.previews.dropboxusercontent.com/p/thumb/AB32uc0OuajSTtut_VdrJDQkOBpXFxmmfsfupIvgUWsd2A3gkDo-S6-BlEhSrZE-23_yMg3aRz9unZ67cL6PGWNbv_m9-MYnHvPNJZr_FtcGMiOVoXafj5CdBeDyew9HEWZ87L2205QBOhZlIKnOc1FCpFN-vOIZdx1iqpgX_R1qcE67_5govvL7zKS7m5Ae73I0dfuHN1L8vDTQT5hrrx3738qBoCVa8FbqEu9_c6dFesGih0m3hdwJiABDtOQG9OoHkGvNmUZFggs72ZCUqbRL0rKFrIHCERgZBHn5Y0_TNHuxPiIxUXZEDyrTbZiE-7mxA_qmCst2wz6-EGiYD4TKsWFms2VUY_ZoX2upicxiKgZVMXqq0_gHPTiflk8Hhaw/p.png";

            if (points > 1100)
                return "https://ucbde18b9794188e27af19579f6a.previews.dropboxusercontent.com/p/thumb/AB2NyTZObUqLtGl0GgZWQU9Adh0bNKod8FZnHdE1Z1LCajAEpHjYHY9m2-u16SoB6Gm3vHAGfQp0bKq1XoPidby1kNlzJUmc6Y3Df2WaDEHoOY7D-9JclgL30-x9V5foNCKnJSt9KLKJDl5AjvyCflEAyui3tp1v62HryXW-6Ddnm_0bo59oj9ZVsY5_j9HruOC9Y2sfzcRMp74QAYDNydHlLr9eJqyAsgeIxEtQxp_6Bn07gMnkscpLkr4abaIoBCRU2jtq27TVAK8phfTKlgsLkpS_PjGelm-QI9XgJ7-gFYDmpC5L3DNTMLO0s_TrjD-kIoeJZNd53NlGpxvcZP6kchfT2LaxL0SCX_D2_VdoGzj9BzF7sBTihTeZ8Ba_TbE/p.png";

            if (points > 1700)
                return "https://uc075569f3256b5b70c2e409ea5b.previews.dropboxusercontent.com/p/thumb/AB15wM57gE3dB3GQS5fo2mRFrYjYRwMkM8NKYUNRuafJMHhIrkod8OxEQlBZKKdoc5W5eLuob9mU4Efz7a3KtgmRqb1v27iaHNBwmkHs9oiaiEXuo1gffbBWEy6zIYERFka_TGKqkwTELl5AxaU6NyPBefhDBaTKIrwgu7TltZXF7GmbGa9gOGilq_1DSY6Zq9z0V6oulTGCrhdIYWZIdNS9BjT-OQG5Tah1hQwjht3ffHvu0HvmN0UXECrx5PJtOG0uBcytSlwbjDpAvHsgx40QiRaU2U13b8yq5v6Y2lJgs-uKFNupW0QHVoUAw3m9VAr4wg8BafKVnf9ED7WDzka2l9ZB2bpL4ZlvnPCQLxeqwHLFgkmug7vkfsXe_xzh3mY/p.png";

            if (points > 2300)
                return "https://uc57926dd66378a7967ce96def9a.previews.dropboxusercontent.com/p/thumb/AB3_WxykN_rZHXJBN8ILrAMVBGbTKYIndHtWpgAj6ehwaf0HC2DKPwdhhVYyNYHMBh_XUeaN2-LyQChjAx-VWZ1gUD1Qp_vTx4HGH6GkU3NL3yxoO4up5n6gMZIYAEriCXmynMDGpQbmAVSERNDB1UxB0J1PV3-G3iv7gdavztSWq-SR7I0rblR0fdCC55fggT2KLvoi-pktnaZrZkXiQfkb3RoBnQPnT0eEC2S4RHKqIYA-6b9QFxl3nJnW-_47cxXrzgoypOwhONoyKKbei2x5MUw2MA0rRSzzW7HsIOxIssMK78yjJAoqn_zk_9aAacsQq8c9fqgO_C_xjIhBtJ8tHohhruRCeVNvd958h8H3AI3ybCHAwWkcVF2K7a23rtw/p.png";

            if (points > 2800)
                return "https://uc12156bbd937597c99de75beebd.previews.dropboxusercontent.com/p/thumb/AB133D0d3FO448iM0ttOqY5GbrEZy6Z3b1i_zaQ-tDz1QYSzaLcd-SEwHCeOLSJscrM_XBMdCorVMezvuevuND5gWU4vZklkYCiHREXxlmZJQLLOaKxnpHA2NvB9pCuR7ZggsWNt4hvtxSC-_rGmQw3aIw4Y-lTR_9ZEs9DksVvj2P0jOecooNiCnTXFe3xNIzUThuoUAeeaustmGlsgzS9TeaYi8OUC8N4y8S95dbcoFFExCLU_8cj5BiSRvUf6Q2H-neT2KlIaiUniD5sOqsIhuv_4xyjytsqmOLi__BbOPzv50uY4PCI1ehwP2EAFYQH_8U4mVVn8xSFzSJ-FbyhCL_6jvzQtQQa54rfBy_vtZUQvE1fv9Gk8j6A9BTdjmg4/p.png";

            if (points > 3200)
                return "https://ucc97f27b8a9ad79ea4695d55b4a.previews.dropboxusercontent.com/p/thumb/AB0k6I1iise3dhtPL5t_1U5FqmT9WNLKTHQeNyS7BedZivxZblyo1QxLo4azJy4TFtnKwTDpFodgpuKQU6Ry1m1d0Dxcn85su82cMcFZSlYj94RY0RGO-uWIr2VAH00MIeAu6Ff9awpSGa4vjmtvvOsCJCfS4N5lZioeZeXHjvQm4Mx25J-YZfsfjyd2mZFoNGmNub4-FyYM4KIup2P12hfvdrLAjtYqDGR61wTG_7I4jrWEc1T1qSrCOk3j1FCta--AEMVFg2bG0zn6YfE-F1AMKf4LVh-ftYICm78yvU-00haz7k28TDncKHpbAawJ59ZXl-yHyBA6wZDkd4gEZktc-xO2WWmvjLI23IqjhByk5nYHkU5lXP8uupehpSFgtDk/p.png";

            if (points > 3700)
                return "https://uc14d101049b9d982fc6f40faad1.previews.dropboxusercontent.com/p/thumb/AB199gUObob2nBn48KQs0wxXiB7zg3wSl4DC22nt_Fr-yTr69yT42PrvI_kBJR7zuguSNbCQYY8m-RFG_VS-IIqBTGKFt1BaVvikWBraRrX1b0GEX7cQ3dwCsKmY3hbJFDW9XEuyZK7fVQ27BeU9y5xZqXePrvw5a89dJYpJM6B7l0vjSnkFQkhPjykeusSxIOqHdGM6WOGBHGS8bnyx-FmogjbLMYCWbOmwlUJecZMR66nt-sxT11IJ0hgpeQlEFHEuzvkuveVSep3289eNmLlSvtnPoBvMZSElt47N4wEOKRhs_8J7lJx7hRXkEUH9kBsTUoW6Wbr_0rTeoFIpp7NvdZqImkFZamKadxuI9We_bYn9JM8x-66etiF22uSekOc/p.png";

            if (points > 4300)
                return "https://ucc9decc1bcf56ad3c139358bb51.previews.dropboxusercontent.com/p/thumb/AB2Ct4E8XaOJbuAeNQJe-W5OUWRnFgzxlGIKVeJ0iYAJoS2Jh2swQEoyIBbokTUuCiXNQPt2ZOL_hQzQ7qIXlua5OjIjj9OKr07ahtSfq_qHxKzkRZXD4cOxWgAdI_4ILuZUyPJYwmwZtDPpJTgr1L7E737F57ZoJ1bh-tEvt8esZ09D4plnXQbnYMniOxkjvlGDTuXlinWel94EnKV-_TRfGuOt4_2ejWcCr-nL-Z7a3qlniNWGFKLsmWSV9MIkcAjjIlSKmAfK2cOBA4ThE76yvYyjyBU2H1QJl1opLW-aUVywDB24JkrOlN_R7KvbSVZXSDCNWru5uiE4D1K6Ur632jrdrv1dkgO4gh_f31hB8aOzZOiTHPxDk3Cn2blQKHU/p.png";

            if (points > 4900)
                return "https://ucc66b6062feb2ed34b98231700a.previews.dropboxusercontent.com/p/thumb/AB3WIPp0qB640NShsLq0DCGZL5rignjobkXlJdPrjPh8YpELtZhddlEHG_z6ULcZC0WhjHIxmiB3AZmKsOPCZaWwH3grsXA1QFNNCUS6c6AlM_PodIVgArAUxgbiP2jhI_U5K7Tdw2_Xuuzy4r0bVkT5wNAtblCRKM8IV_rSlwHpFbcUjunqAO4vDo_3bdHf4JGXzR90A6zmvCFIyog0J8_gXjV7-saTd1zw78q4MVCMy8Df4S_rELGKU-3wwFu5A7szd2frxKuZm93pfuBW8luC3nbHC2Ae7wOeBppaIZ1zY95XlpFzLfAGj7vK4Q5eJKcpKQIo8o0JNAAJN2VCA5i-5JVPn4PrTwbPS4ExkZD8HQEdGHGop_CF6d0dMO6g0og/p.png";

            if (points > 5200)
                return "https://uca21e96c5b98e1aede3d30c61dc.previews.dropboxusercontent.com/p/thumb/AB0Tgzv_Bg8st9CDzRIt_utDRBp4tBtS6Qzx3kxX37Vuw5YL_aO2kyKjrq8SbsknN5wEueQQSc3LLh4JDjEoTFCNLrqG-Co7wbhFz4qz-PN4BaOpYDEBMxVz-ghDAlrJDJOyVicR40YHRV4IHGx9eighgP1btrrio_s77dLJsJfq8ij5MEpCl5JIAqulWk4hTdtDnIaZsfhNnvEb5kBHs7zg8dAl1M8seEGVRzN5_EQiTaERj3GShqhs_NXshTUDjBN_2yD7tJ_06bas6KxhuDnY7gppJQiTAohFfBTX58uhfgcfPxdZq1jEPqcv0zz_wUBhn0SG9IpZ_RH_Km8lGqWgfQwJwrxGW0ApZLp9otKg4SUlMYMwH_dbl-1_ZnfXQX8/p.png";

            if (points > 5700)
                return "https://uc87b9096954d7646a7a384b6ae9.previews.dropboxusercontent.com/p/thumb/AB2c6JrRfhyv8nZD0dbggDpTYSuldIa-sEC0DhSTNGl-5dGMwtXuDeKvCG0WeUoF1Y1gaNEhdtMcOLCKlr4z1YdFfpkAZkyS-L_V-XN3cMJQRc6HAYUgSRqfwn5Q7HqndxujNPOYaV9SA5UH1EHggXe6oE0-j-4D32RtHOD-t-pIQZEwW9UtR-zBkIm4jc1_wrOVgjYHHBuxfYZ4iIvjGAwhbUSRtoVMbuwj-q56xh9RFoWfh0aynoPBPm6pWFXd4ojKfCQz2LLZDtpS4uH6NpwJVLnDPNmP9-PBGmzATQqSiPO-ul7YyH8dqg4L6-ybuPyt5B-ZfTqZOX8bGBib9o1nzF6fivhCK4p8VxeeC1wxksHdgDAI_9N3eaLqOc9pg5A/p.png";

            if (points > 6300)
                return "https://uccd5ac759cd2dca61551a5035d4.previews.dropboxusercontent.com/p/thumb/AB3T_1riUdCjA7nF-MmMJ4nqHMG2VE79Ls6BFROoMgeMZFm2t4LVBLhXl_NSgE3Tr81wZSxVcDTGbWyeSkdDssaijprY0Zlg3yTbQHWQZ6u8R8cf__F0lCCyDnThq_-nwbfQw1BIqSqboSi9_fuUJFWHoJwsNa1ZZpN8wJEFQqLcylctBNrdQIh9J_tKEk1i-Y_Biq0tJh_F4RVCcCKapl5_0M4MYS9kSMJ2U1Yv8jcrVIqbw2dz0n71FkoyLwiMCfnNZQCl1ECQooULwEaXVUORlJsQCGm3mVw4NLvwhKLUISynPttUEjPik41xAe4k0otSQnHTBSnKPsO3TWKwioYmB-FvnQRprn9eDcURglusnO8R5WIcW3YYef59Y3ya8Ps/p.png";

            if (points > 6800)
                return "https://ucae6486ff7e17ac9be824002078.previews.dropboxusercontent.com/p/thumb/AB2mbsS1-533BuagL-TkI83MRoLFJRh6LF89SXK2_tbFHlUd0954v4FzXoEGJl4pvQNGqJPwV3Tf4HJtUv_CZ74OFkhG5vgp4LvPPd0Ws2rMOmBmu0BmLVtrrxWJHCxtJvfCVYFZVJb6NxuYs2vsUaGqBYYw8g-Br_nqwMb0zDZ6T9SoGEN2p7ISeiGwLkhcv8BeL89GN_RfxmIADEimnqdYxTJRvcag37NOne1qh3Kq5f5OyIVVufwp8xqA-8pAV4vIZOFgKjdWdECXYOSreAGppgjSS2lL6idIhmK2nExlpNUG0f4neRkPl82ZGIUI28T3BBzRapSJ3Sj7B4-NYW8PKCT1PAHDrPX-SscDsa_KSDY3TOx0Du10kGXA1GL9Esc/p.png";

            if (points > 7300)
                return "https://uc7ce8d2628ea2bd0a3cf9db6e5a.previews.dropboxusercontent.com/p/thumb/AB2lIMrS5gtVqPx01fke_fGi0izNUhruxqT2ao8BgHjc261IEg5pT9uzxW4_CnQAU7vs2RVNXWbupvt7U3U0CtTOxY602cTJvo0aIUfJZf21792hc8TN7JF0pI0LSYXZEdU61HG0meq8_U7Mm9_0TrmPBwHExAc231XWIgtGO6sDrb0hIWoDwLSURGPLv3exQhfOU98c3L9mUItly4_iPxJSL7EBmPYFzidy9C89NUI9CQQuBw69_XPd0rk-NhrB1gDUtXsPojR7SjsGdneuit2FipGj-3049mEKzIHQ1bMOAGkfDG4Mn-OogD1gPvanDUCHrZWPUpQOn5v2I7TJmOeVlVqEl3JlkzzLMbFtbWERXX6UXC1WqduMdW4CuHWzOqo/p.png";

            if (points > 7800)
                return "https://ucb666432f76d03234fa9af13c17.previews.dropboxusercontent.com/p/thumb/AB0TTDOoVSgq1J7uws1YDZS012gwWddSxWmEbY_ZE1b108HcnyFSr_3Csqj9MS6HmAhDYL23WTpJ6RBP4SSCkqODQj2WJco-pxIi9md6D6Z_g8O2UABBhpD8VxryXyKk5K8BhMkuplerzwE8C2n10HfsSjBK2Jq6_Tty6UyIgHdOLG9X8mdpK7Y4nypcN9YayRCZlrpQduR0FCmQvOeSE2NIwLPGDlZKpaSumOXhPa1lFcH-tBqm70qxHVp3ZxTuuiolX0pwTLAp5uoi3ZjRyX6XpI4oxsRQQERB-qxqlrdTnbjPLgRUNDLuUBGxcWut8m56qLTgXGN2fx6E1JzwzRwwshhWxGh1-dzIK25In8rpfL66L0jfM9dRB6fqfoKowKw/p.png";

            if (points > 8300)
                return "https://ucc748bc1b4009206fa3a27de848.previews.dropboxusercontent.com/p/thumb/AB1loyCy0c5vs0dNzAcV-BdCPUBf9qgMLn41mMQJgEnbkviZynrYcoF5rrnkeZcWmJeQt8iSsddbaOtmdgphGefxFt0fs8NCfXu7JCD9DBr_vjHO693vzjT1SQ-28r9sc58kQrT3_E7lDKCis-bomdiPKQmC0oW8UPWBAe_kDZedqu0mFVJeu-_rUYuwUMEOo79Ao1lF5KNNv8GrucashVUy60BAbl4NxKw1FjA5nK3TGyBi2dbiITpLTjdygTNitc7x20ry80sPDzQnuPf_Wqh_QoA8jypsBWyNeHHSLoLLg6WLXPA0Yb3h2UBcFZkc2Epakx4gH-KFX8i9bGU-33ROPE-NqYAQ5wAd12IKaF1dziMub5qX7xMnQJXvrfnn8f4/p.png";

            if (points > 10000)
                return "https://ucaed54e06a2b1ed799810c9ad6f.previews.dropboxusercontent.com/p/thumb/AB0f-Gecw42WO_YSgomklWP9L23vWQOKaznzBBIFRo4kqE22bqkz7wOg_PkJvE-lIQ4yuVGqszE0ErSxlAJWtef3jXDK-a0J2w7Uh4yT5gY6zTi_9C-VhqwqGs-U387r97gDTmcioY2xl9i3hJkfyz9ieQv26q6ICueiwCcwQP8-y2mIZ6KrtiVLyuZq-h_d4JQMJlCbBkkWQK-W-6gY-rxZO8DpjMm0z6eT76bnIgQRsseH2-apTuAaO4QRZhiLRaQf4QaKg4Md0DOYpVA1aeliqQl86k0hEmKD4o4XSnjWPmvGtUUJNWC5bx2XqocXJTs9AymnqdKS--_saYde4w3ACtahZEd_LnwcWyPFEYpjSsdkSy4qtvlsic97EwN35kI/p.png";

            return "";
        }
    }
}
