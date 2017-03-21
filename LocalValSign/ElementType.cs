using System;
using System.Collections.Generic;
using System.Text;

namespace LocalValSign
{
    public enum ElementType
    {
        /// <summary>
        /// Marks end of a list
        /// </summary>
        End = 0x00,   // Marks end of a list
        /// <summary>
        /// System.Void
        /// </summary>
        Void = 0x01,
        /// <summary>
        /// System.Boolean
        /// </summary>
        Boolean = 0x02,
        /// <summary>
        /// System.Char
        /// </summary>
        Char = 0x03,
        /// <summary>
        /// System.SByte
        /// </summary>
        I1 = 0x04,
        /// <summary>
        /// System.Byte
        /// </summary>
        U1 = 0x05,
        /// <summary>
        /// System.Int16
        /// </summary>
        I2 = 0x06,
        /// <summary>
        /// System.UInt16
        /// </summary>
        U2 = 0x07,
        /// <summary>
        /// System.Int32
        /// </summary>
        I4 = 0x08,
        /// <summary>
        /// System.UInt32
        /// </summary>
        U4 = 0x09,
        /// <summary>
        /// System.Int64
        /// </summary>
        I8 = 0x0a,
        /// <summary>
        /// System.UInt64
        /// </summary>
        U8 = 0x0b,
        /// <summary>
        /// System.Single
        /// </summary>
        R4 = 0x0c,
        /// <summary>
        /// System.Double
        /// </summary>
        R8 = 0x0d,
        /// <summary>
        /// System.String
        /// </summary>
        String = 0x0e,
        /// <summary>
        /// Unmanaged pointer, followed by the Type element.
        /// </summary>
        Ptr = 0x0f,   // Followed by <type> token
        /// <summary>
        /// Managed pointer, followed by the Type element.
        /// </summary>
        ByRef = 0x10,   // Followed by <type> token
        /// <summary>
        /// A value type modifier, followed by TypeDef or TypeRef token
        /// </summary>
        ValueType = 0x11,   // Followed by <type> token
        /// <summary>
        /// A class type modifier, followed by TypeDef or TypeRef token
        /// </summary>
        Class = 0x12,   // Followed by <type> token
        /// <summary>
        /// Generic parameter in a generic type definition, represented as number
        /// </summary>
        Var = 0x13,   // Followed by generic parameter number
        /// <summary>
        /// A multi-dimensional array type modifier.
        /// Declaration		    Type	Rank	NumSizes	Size	NumLoBounds	LoBound
        ///[0...2]			    I4	    1	    1		    3	    0		    -
        ///[,,,,,,]		        I4	    7	    0		    -	    0		    -
        ///[0...3, 0...2,,,,]	I4	    6	    2		    4 3	    2		    0 0
        ///[1...2, 6...8]		I4	    2	    2		    2 3	    2		    1 6
        ///[5, 3...5, , ]		I4	    4	    2		    5 3	    2		    0 3
        /// </summary>
        Array = 0x14,   // <type> <rank> <boundsCount> <bound1>  <loCount> <lo1>
        /// <summary>
        /// Generic type instantiation. Followed by type type-arg-count type-1 ... type-n
        /// </summary>
        GenericInst = 0x15,   // <type> <type-arg-count> <type-1> ... <type-n> */
        /// <summary>
        /// A typed reference.
        /// </summary>
        TypedByRef = 0x16,
        /// <summary>
        /// System.IntPtr
        /// </summary>
        I = 0x18,   // System.IntPtr
        /// <summary>
        /// System.UIntPtr
        /// </summary>
        U = 0x19,   // System.UIntPtr
        /// <summary>
        /// A pointer to a function, followed by full method signature
        /// </summary>
        FnPtr = 0x1b,   // Followed by full method signature
        /// <summary>
        /// System.Object
        /// </summary>
        Object = 0x1c,   // System.Object
        /// <summary>
        /// A single-dimensional, zero lower-bound array type modifier.
        /// </summary>
        SzArray = 0x1d,   // Single-dim array with 0 lower bound
        /// <summary>
        /// Generic parameter in a generic method definition, represented as number
        /// </summary>
        MVar = 0x1e,   // Followed by generic parameter number
        /// <summary>
        /// Required modifier, followed by a TypeDef or TypeRef token
        /// </summary>
        CModReqD = 0x1f,   // Required modifier : followed by a TypeDef or TypeRef token
        /// <summary>
        /// Optional modifier, followed by a TypeDef or TypeRef token
        /// </summary>
        CModOpt = 0x20,   // Optional modifier : followed by a TypeDef or TypeRef token
        /// <summary>
        /// Implemented within the CLI
        /// </summary>
        Internal = 0x21,   // Implemented within the CLI
        /// <summary>
        /// ORed with following element types
        /// </summary>
        Modifier = 0x40,   // Or'd with following element types
        /// <summary>
        /// Sentinel for vararg method signature
        /// </summary>
        Sentinel = 0x41,   // Sentinel for varargs method signature
        /// <summary>
        /// Denotes a local variable that points at a pinned object
        /// </summary>
        Pinned = 0x45,   // Denotes a local variable that points at a pinned object

        // special undocumented constants

        /// <summary>
        /// Indicates an argument of type System.Type.
        /// </summary>
        Type = 0x50,
        /// <summary>
        /// Used in custom attributes to specify a boxed object (§23.3 in the ECMA-355 specification).
        /// </summary>
        Boxed = 0x51,

        /// <summary>
        /// Used in custom attributes to specify an enum (§23.3 in the ECMA-355 specification).
        /// </summary>
        Enum = 0x55
    }
}
