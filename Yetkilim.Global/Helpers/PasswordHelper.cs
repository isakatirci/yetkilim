// Decompiled with JetBrains decompiler
// Type: Yetkilim.Global.Helpers.PasswordHelper
// Assembly: Yetkilim.Global, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64E10379-6F96-4F72-8B19-6E8446119219
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Global.dll

using System;
using System.Security.Cryptography;
using System.Text;

namespace Yetkilim.Global.Helpers
{
  public class PasswordHelper
  {
    public static string MD5Hash(string input)
    {
      using (MD5 md5 = MD5.Create())
        return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", string.Empty).ToLower();
    }

    public static string GeneratePassword(int length)
    {
      string[] strArray = ("a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z," + "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z," + "1,2,3,4,5,6,7,8,9,0,!,@,#,$,%,&,?").Split(',');
      string str1 = "";
      Random random = new Random();
      for (int index = 0; index < length; ++index)
      {
        string str2 = strArray[random.Next(0, strArray.Length)];
        str1 += str2;
      }
      return str1;
    }
  }
}
