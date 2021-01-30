// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using System;

namespace EnhancedEditor
{
	public static class ArrayExtensions
    {
        #region Content
        /// <summary>
        /// Adds an element to an array.
        /// </summary>
        /// <typeparam name="T">Array content type.</typeparam>
        /// <param name="_source">Array where to add element.</param>
        /// <param name="_element">Element to add.</param>
        /// <returns>Returns array with new element.</returns>
        public static T[] Add<T>(this T[] _source, T _element)
        {
            T[] _newArray = new T[_source.Length + 1];
            for (int _i = 0; _i < _source.Length; _i++)
                _newArray[_i] = _source[_i];

            _newArray[_source.Length] = _element;
            return _newArray;
        }

        /// <summary>
        /// Remove an element from an array at index.
        /// </summary>
        /// <typeparam name="T">Array content type.</typeparam>
        /// <param name="_source">Array from to remove element.</param>
        /// <param name="_index">Element to remove.</param>
        /// <returns>Returns array without element.</returns>
        public static T[] RemoveAt<T>(this T[] _source, int _index)
        {
            T[] _new = new T[_source.Length - 1];

            Array.Copy(_source, _new, _index);
            if (_index < _new.Length)
                Array.Copy(_source, _index + 1, _new, _index, _new.Length - _index);

            return _new;
        }

        /// <summary>
        /// Try to find an element within a array.
        /// </summary>
        /// <typeparam name="T">Array content type.</typeparam>
        /// <param name="_source">Array to check content.</param>
        /// <param name="_match">Prediction to find wanted element.</param>
        /// <param name="_result">Matching element.</param>
        /// <returns>Returns true if found, false otherwise.</returns>
        public static bool Find<T>(this T[] _source, Predicate<T> _match, out T _result)
        {
            for (int _i = 0; _i < _source.Length; _i++)
            {
                T _element = _source[_i];
                if (_match(_element))
                {
                    _result = _element;
                    return true;
                }
            }

            _result = default(T);
            return false;
        }
        #endregion
    }
}
