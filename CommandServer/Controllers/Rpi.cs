using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;

namespace WebApplication1.Controllers;

public class Rpi : Controller
{
    private const int HexSize = 2; //number of hex digits per motor
    private static float Modulo(float n, float r) => (n % r + r) % r;
    
    /// <summary>
    /// Decodes from Hexadecimal (easily convertable to UTF7, 8, or 16 in future)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static ushort HexToInt(string value)
    {
        ushort result = 0;
        char[] arr = value.ToCharArray();
        for (int i = arr.Length - 1; i > -1; i--)
            result += (ushort) (arr[i] * (128 ^ i));

        return result;
    }

    public static ushort[] HexToArr(string input)
    {
        List<ushort> arr = new List<ushort>();
        for (int i = 0; i < input.Length; i += HexSize)
            arr.Add(HexToInt(input.Substring(i, HexSize)));

        return arr.ToArray();
    }

    /// <summary>
    /// Linearly maps the unsigned curl parameters () to signed angles for a motor
    /// </summary>
    /// <param name="measure"></param>
    /// <param name="lowerBound"></param>
    /// <param name="upperBound"></param>
    /// <returns></returns>
    public static float DecodeAngle(ushort measure, float lowerBound, float upperBound)
    {
        float result = measure * (upperBound - lowerBound); //Scale measure
        return result - lowerBound; //Offset measure
    }
    
    // GET
    /// <summary>
    /// Set power level of drive system
    /// </summary>
    /// <param name="power">Ranges from -100 to 100</param>
    /// <returns></returns>
    public static void SetSpeed(float power)
    {
        Console.WriteLine($"SPEED SET TO: {power}");
    }

    public static void SetSteering(float angle)
    {
        Console.WriteLine($"STEERING ANGLE SET TO: {angle}");
    }
    
}