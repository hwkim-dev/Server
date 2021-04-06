﻿using System;
using System.Collections.Generic;
using System.Text;
using LOGIN_DATA;

namespace Login_Server
{
    [Flags]
    enum SendFormCode
    {
        SIGNUP = 0b00000001,
        LOGIN = 0b00000010,
        FINDID = 0b00000011,
        FINDPW = 0b00000100,
        CHANGEID = 0b00000101,
        CHANGEPW = 0b00000111,
        DELETEACCOUNT = 0b00001000,
        EMAILVERTIFY = 0b00001001,
        IDOVERLAP = 0b00001011,
        NICKOVERLAP = 0b00001100,
        EMAILOVERLAP = 0b00001101,
        ISINPUTCORRECT = 0b00001111,
    }
    enum Purpose
    {
        CHECKPW = 0b10000000,
    }

    static class Form
    {
        public static unsafe void chech_From(char* _rcvdata, byte* _return_To_Client)
        {
            switch ((byte)_rcvdata[0])
            {
                case (byte)SendFormCode.SIGNUP:
                    LOGIN_SQL.sign_Up(_rcvdata, _return_To_Client);
                    break;
                case (byte)SendFormCode.LOGIN:
                    LOGIN_SQL.login(_rcvdata, _return_To_Client);
                    break;
                case (byte)SendFormCode.FINDID:
                    LOGIN_SQL.find_Id(_rcvdata, _return_To_Client);
                    break;
                case (byte)SendFormCode.FINDPW:
                    LOGIN_SQL.find_Pw(_rcvdata, _return_To_Client);
                    break;
                case (byte)SendFormCode.CHANGEID:
                    LOGIN_SQL.change_Id(_rcvdata, _return_To_Client);
                    break;
                case (byte)SendFormCode.CHANGEPW:
                    LOGIN_SQL.change_Pw(_rcvdata, _return_To_Client);
                    break;
                case (byte)SendFormCode.DELETEACCOUNT:
                    LOGIN_SQL.delete_Accoutnt(_rcvdata, _return_To_Client);
                    break;
                case (byte)SendFormCode.EMAILVERTIFY:
                    LOGIN_SQL.email_Vertify(_rcvdata, _return_To_Client);
                    break;
                case (byte)SendFormCode.IDOVERLAP:
                    LOGIN_SQL.id_Overlap(_rcvdata, _return_To_Client);
                    break;
                case (byte)SendFormCode.NICKOVERLAP:
                    LOGIN_SQL.nick_Overlap(_rcvdata, _return_To_Client);
                    break;
                case (byte)SendFormCode.EMAILOVERLAP:
                    LOGIN_SQL.email_Overlap(_rcvdata, _return_To_Client);
                    break;
                case (byte)SendFormCode.ISINPUTCORRECT:
                    LOGIN_SQL.is_Input_Correct(_rcvdata, _return_To_Client);
                    break;
                default:

                    break;
            }
        }
        /*
        static char[] id;
        static char[] pw;
        static char[] email;
        static char[] nickname;
        static int input;

        static string txt;
        public static unsafe bool check_Form(byte* income,string _txt)
        {
            id = new char[12];
            pw = new char[12];
            email = new char[25];
            nickname = new char[12];

            txt = _txt;
            

            //!!correct -> is inputcorrect에서 불러서 사용 -> 이메일 인증할때 6자리 코드!!
            int correct;
            //SQL -> 성공여부
            bool succed = true;
            Console.WriteLine((byte)SendFormCode.Login);
            switch (*income)
            {
                case (byte)SendFormCode.SignUp:
                    //포인터 사용 O -> 4, 4 = 8
                    //포인터 사용 X -> 4, 4+1, 4 = 12
                    fixed (char* _id = id) fixed (char* _pw = pw) fixed (char* _email = email) fixed (char* _nickname = nickname)
                    {
                        Cut_String(_id, _pw, _email, _nickname);
                        LOGIN_SQL.SignUp(_id, _pw, _email, _nickname, &succed);
                    }
                    break;
                case (byte)SendFormCode.Login:
                    Console.WriteLine("4");
                    fixed (char* _id = id) fixed (char* _pw = pw)
                    {
                        Console.WriteLine("5");
                        Cut_String(_id, _pw);
                        Console.WriteLine("7");
                        LOGIN_SQL.Login(_id, _pw, &succed);
                        Console.WriteLine("8" + succed);
                    }
                    break;
                case (byte)SendFormCode.isInputCorrect:
                        LOGIN_SQL.isInputCorrect(&correct, &succed);
                    break;
                case (byte)SendFormCode.id_Overlap:
                    fixed (char* _id = id)
                    {
                        Cut_String(_id);
                        LOGIN_SQL.id_Overlap(_id, &succed);
                    }
                    break;
                case (byte)SendFormCode.nick_Overlap:
                    fixed (char* _nick = nickname)
                    {
                        Cut_String(_nick);
                        LOGIN_SQL.nick_Overlap(_nick, &succed);
                    }
                    break;
                case (byte)SendFormCode.email_Overlap:
                    fixed (char* _email = email)
                    {
                        Cut_String(_email);
                        LOGIN_SQL.email_Overlap(_email, &succed);
                    }
                    break;
                case (byte)SendFormCode.FindPW:
                    fixed (char* _id = id) fixed (char* _email = email)
                    {
                        Cut_String(_id, _email);
                        LOGIN_SQL.FindPW(_id, _email, &correct, &succed);
                    }
                    break;
                case (byte)SendFormCode.FindID:
                    fixed (char* _email = email) fixed (char* _id = id)
                    {
                        //id는 리턴해서 받을 값임
                        Cut_String(_email);
                        LOGIN_SQL.FindID(_id, _email, &succed);
                    }
                    break;
                case (byte)SendFormCode.DeleteAccount:
                    fixed (char* _id = id) fixed (char* _pw = pw)
                    {
                        Cut_String(_id, _pw);
                        LOGIN_SQL.DeleteAccount(_id, _pw, &succed);
                    }
                    break;
                case (byte)SendFormCode.ChangePW:
                    fixed (char* _id = id) fixed (char* _newpw = pw)
                    {
                        //pw -> new password
                        Cut_String(_id, _newpw);
                        LOGIN_SQL.ChangePW(_id, _newpw, &succed);
                    }
                    break;
                case (byte)SendFormCode.ChangeID:
                    fixed (char* _id = id) fixed (char* _newid = pw)
                    {
                        //id -> new id
                        Cut_String(_id, _newid);
                        LOGIN_SQL.ChangeID(_id, _newid, &succed);
                    }
                    break;
                default:

                    break;
            }

            return false;
        }

        // byte배열로 정보를 통신할때
        // null으로 정보를 구별
        // ex) ['h','e','l','l','o','\0','6','4','3','1']
        // hello가 첫번째 정보가되고. Nullcharacter를 만나서 다음으로 넘어가
        // 6431이 두번째 정보가된다.
        private static unsafe void Cut_String(char* first)
        {
            int leng = (txt.Length - 1);
            int i = 0;
            int j = 0;
            while (txt[i] == '\0') { first[j] = txt[i]; ++j; ++i; }
        }
        //오버라이딩
        private static unsafe void Cut_String(char* first, char* second)
        {
            int leng = (txt.Length - 1);
            int i = 0;
            int j = 0;

            // *first = txt.ToCharArray(1,2); <- X
            //first[j] <- txt[i] i와j같이 커짐 
            // !! j 루프 끝날때마다 초기화해서 !!
            //왼쪽 배열은 0부터 // 오른쪽 배열은 값유지
            while (txt[i] == '\0') { first[j] = txt[i]; ++j; ++i; }
            ++i; j = 0;
            while (txt[i] == '\0') { second[j] = txt[i]; ++j; ++i; }
            ++i; j = 0;
            Console.WriteLine("6");
        }
        //오버라이딩
        private static unsafe void Cut_String(char* first, char* second, char* third)
        {
            int leng = (txt.Length - 1);
            int i = 0;
            int j = 0;

            while (txt[i] == '\0') { first[j] = txt[i]; ++j; ++i; }
            ++i; j = 0;
            while (txt[i] == '\0') { second[j] = txt[i]; ++j; ++i; }
            ++i; j = 0;
            while (txt[i] == '\0') { third[j] = txt[i]; ++j; ++i; }
            ++i; j = 0;
        }
        private static unsafe void Cut_String(char* first, char* second, char* third, char* fourth)
        {
            int leng = (txt.Length - 1);
            int i = 0;
            int j = 0;

            while (txt[i] == '\0') { first[j] = txt[i]; ++j; ++i; }
            ++i; j = 0;
            while (txt[i] == '\0') { second[j] = txt[i]; ++j; ++i; }
            ++i; j = 0;
            while (txt[i] == '\0') { third[j] = txt[i]; ++j; ++i; }
            ++i; j = 0;
            while (i < leng) { fourth[j] = txt[i]; ++j; ++i; }
            ++i; j = 0;
        }

        private static unsafe void reset_in(char* first)
        {

        }
        private static unsafe void reset_in(char* first, char* second)
        {

        }
        private static unsafe void reset_in(char* first, char* second, char* third)
        {

        }
        private static unsafe void reset_in(char* first, char* second, char* third, char* fourth)
        {

        }*/
    }
}
