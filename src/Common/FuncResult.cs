using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace datatablegenerator.Common
{ 
    //
    // 摘要:
    //     方法返回值
    //
    // 类型参数:
    //   T:
    //     返回内容数据类型
    [DebuggerDisplay("Code={Code},Message={Message},Data={Data}")]
    public class FuncResult<T> : FuncResult
    {
        //
        // 摘要:
        //     返回的内容
        public T Data { get; set; }

        //
        // 摘要:
        //     T data 隐式转换 FuncResult<T>.SuccessResult(data)
        //
        // 参数:
        //   data:
        public static implicit operator FuncResult<T>(T data)
        {
            return FuncResult.Success(data);
        }
    }


    //
    // 摘要:
    //     方法返回实体
    //
    // 言论：
    //     此对象通常用于当一个方法需要返回多种数据时使用
    [DebuggerDisplay("Code={Code},Message={Message}")]
    public class FuncResult
    {
        //
        // 摘要:
        //     失败的编码
        public const string _FAIL_CODE = "0500";

        //
        // 摘要:
        //     成功的编码
        public const string _SUCCESS_CODE = "0000";

        //
        // 摘要:
        //     成功的默认信息
        public const string _SUCCESS_MESSAGE = "操作成功";

        //
        // 摘要:
        //     状态码
        public string Code { get; set; }

        //
        // 摘要:
        //     错误信息
        public string Message { get; set; }

        //
        // 摘要:
        //     [返回一个成功的，带有数据的FuncResult<T>对象
        //
        // 参数:
        //   content:
        //     返回内容
        //
        //   message:
        //     返回信息
        //
        // 类型参数:
        //   T:
        //     返回内容的类型
        //
        // 返回结果:
        //     [返回一个成功的，带有数据的FuncResult<T>对象
        public static FuncResult<T> Success<T>(T content, string message = null)
        {
            return new FuncResult<T>
            {
                Data = content,
                Code = "0000",
                Message = (message ?? "操作成功")
            };
        }

        //
        // 摘要:
        //     返回一个成功的FuncResult对象
        //
        // 返回结果:
        //     返回一个成功的FuncResult对象
        public static FuncResult Success(string message = null)
        {
            return new FuncResult
            {
                Code = "0000",
                Message = (message ?? "操作成功")
            };
        }

        //
        // 摘要:
        //     返回失败
        //
        // 参数:
        //   data:
        //     返回内容
        //
        //   message:
        //     错误信息
        //
        //   code:
        //     返回码
        //
        // 类型参数:
        //   T:
        //     返回内容的类型
        //
        // 返回结果:
        //     [返回一个失败的FuncResult<T>对象
        public static FuncResult<T> Fail<T>(T data, string message, string code = "0500")
        {
            return new FuncResult<T>
            {
                Data = data,
                Message = message,
                Code = code
            };
        }

        //
        // 摘要:
        //     返回失败
        //
        // 参数:
        //   message:
        //     错误信息
        //
        //   code:
        //     返回码
        //
        // 类型参数:
        //   T:
        //     返回内容的类型
        //
        // 返回结果:
        //     [返回一个失败的FuncResult<T>对象
        public static FuncResult<T> Fail<T>(string message, string code = "0500")
        {
            return Fail(default(T), message, code);
        }

        //
        // 摘要:
        //     返回失败
        //
        // 参数:
        //   message:
        //     错误信息
        //
        //   code:
        //     返回码
        //
        // 返回结果:
        //     返回一个失败的FuncResult对象
        public static FuncResult Fail(string message, string code = "0500")
        {
            return new FuncResult
            {
                Message = message,
                Code = code
            };
        }

        //
        // 摘要:
        //     返回result的Success
        //
        // 参数:
        //   result:
        public static implicit operator bool(FuncResult result)
        {
            if (result != null)
            {
                return result.Code == "0000";
            }

            return false;
        }
    }

}
