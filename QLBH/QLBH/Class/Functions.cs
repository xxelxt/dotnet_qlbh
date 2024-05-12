using System;

namespace QLBH.Class
{
    internal static class Functions
    {
        public static bool IsDate(string d)
        {
            //string[] parts = d.Split('/');

            //if ((Convert.ToInt32(parts[0]) >= 1) && (Convert.ToInt32(parts[0]) <= 31) &&
            //    (Convert.ToInt32(parts[1]) >= 1) && (Convert.ToInt32(parts[1]) <= 12) && 
            //    (Convert.ToInt32(parts[2]) >= 1900))
            //    return true;
            //else
            //    return false;

            string[] parts = d.Split('/');

            if (parts.Length != 3)
                return false;

            int day, month, year;
            if (!int.TryParse(parts[0], out day) || !int.TryParse(parts[1], out month) || !int.TryParse(parts[2], out year))
                return false;

            if (day < 1 || day > 31 || month < 1 || month > 12 || year < 1900)
                return false;

            if ((month == 4 || month == 6 || month == 9 || month == 11) && day > 30)
                return false;

            if (month == 2)
            {
                bool isLeapYear = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
                if ((isLeapYear && day > 29) || (!isLeapYear && day > 28))
                    return false;
            }

            return true;
        }


        public static string ConvertDateTime(string d)
        {
            string[] parts = d.Split('/');

            if (parts.Length != 3)
                throw new ArgumentException("Định dạng ngày tháng không hợp lệ.");

            string dt = String.Format("{0}/{1}/{2}", parts[1], parts[0], parts[2]);
            return dt;
        }

        public static string ChuyenSoSangChu(string sNumber)
        {
            int mLen, mDigit;
            string mTemp = "";
            string[] mNumText;
            //Xóa các dấu "," nếu có
            sNumber = sNumber.Replace(",", "");
            mNumText = "không;một;hai;ba;bốn;năm;sáu;bảy;tám;chín".Split(';');
            mLen = sNumber.Length - 1; // trừ 1 vì thứ tự đi từ 0
            for (int i = 0; i <= mLen; i++)
            {
                mDigit = Convert.ToInt32(sNumber.Substring(i, 1));
                mTemp = mTemp + " " + mNumText[mDigit];
                if (mLen == i) // Chữ số cuối cùng không cần xét tiếp
                    break;
                switch ((mLen - i) % 9)
                {
                    case 0:
                        mTemp = mTemp + " tỷ";
                        if (sNumber.Substring(i + 1, 3) == "000")
                            i = i + 3;
                        if (sNumber.Substring(i + 1, 3) == "000")
                            i = i + 3;
                        if (sNumber.Substring(i + 1, 3) == "000")
                            i = i + 3;
                        break;
                    case 6:
                        mTemp = mTemp + " triệu";
                        if (sNumber.Substring(i + 1, 3) == "000")
                            i = i + 3;
                        if (sNumber.Substring(i + 1, 3) == "000")
                            i = i + 3;
                        break;
                    case 3:
                        mTemp = mTemp + " nghìn";
                        if (sNumber.Substring(i + 1, 3) == "000")
                            i = i + 3;
                        break;
                    default:
                        switch ((mLen - i) % 3)
                        {
                            case 2:
                                mTemp = mTemp + " trăm";
                                break;
                            case 1:
                                mTemp = mTemp + " mươi";
                                break;
                        }
                        break;
                }
            }
            //Loại bỏ trường hợp x00
            mTemp = mTemp.Replace("không mươi không ", "");
            mTemp = mTemp.Replace("không mươi không", "");
            //Loại bỏ trường hợp 00x
            mTemp = mTemp.Replace("không mươi ", "linh ");
            //Loại bỏ trường hợp x0, x>=2
            mTemp = mTemp.Replace("mươi không", "mươi");
            //Fix trường hợp 10
            mTemp = mTemp.Replace("một mươi", "mười");
            //Fix trường hợp x4, x>=2
            mTemp = mTemp.Replace("mươi bốn", "mươi tư");
            //Fix trường hợp x04
            mTemp = mTemp.Replace("linh bốn", "linh tư");
            //Fix trường hợp x5, x>=2
            mTemp = mTemp.Replace("mươi năm", "mươi lăm");
            //Fix trường hợp x1, x>=2
            mTemp = mTemp.Replace("mươi một", "mươi mốt");
            //Fix trường hợp x15
            mTemp = mTemp.Replace("mười năm", "mười lăm");
            //Bỏ ký tự space
            mTemp = mTemp.Trim();
            //Viết hoa ký tự đầu tiên
            mTemp = mTemp.Substring(0, 1).ToUpper() + mTemp.Substring(1) + " đồng";
            return mTemp;
        }

        public static string ConvertTimeTo24(string hour)
        {
            string h = "";
            switch (hour)
            {
                case "1":
                    h = "13";
                    break;
                case "2":
                    h = "14";
                    break;
                case "3":
                    h = "15";
                    break;
                case "4":
                    h = "16";
                    break;
                case "5":
                    h = "17";
                    break;
                case "6":
                    h = "18";
                    break;
                case "7":
                    h = "19";
                    break;
                case "8":
                    h = "20";
                    break;
                case "9":
                    h = "21";
                    break;
                case "10":
                    h = "22";
                    break;
                case "11":
                    h = "23";
                    break;
                case "12":
                    h = "0";
                    break;
            }
            return h;
        }
    }
}
