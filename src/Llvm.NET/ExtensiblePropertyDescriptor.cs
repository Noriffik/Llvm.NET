﻿using System;

namespace Llvm.NET
{
    /// <summary>Provides consistent accessors for an extended property</summary>
    /// <typeparam name="T">Type of values stored in the property</typeparam>
    /// <remarks>
    /// This class is used to describe a property stored in a class implementing
    /// <see cref="IExtensiblePropertyContainer"/>. Using a single, typically
    /// <see langword="static"/>, instance of this class to describe and access
    /// an extended property helps to encapsulate the type casting and property
    /// ID into a single place. Making calling code easier to comprehend and
    /// less prone to typographical errors that a compiler can't catch ahead of
    /// time.
    /// </remarks>
    public class ExtensiblePropertyDescriptor<T>
    {
        /// <summary>Creates a new instance of a property descriptor</summary>
        /// <param name="name">Name of the extended property</param>
        public ExtensiblePropertyDescriptor( string name )
        {
            Name = name;
        }

        /// <summary>Gets a value for the property from the container</summary>
        /// <param name="container">container</param>
        /// <returns>Value retrieved from the property or the default value of type <typeparamref name="T"/></returns>
        public T GetValueFrom( IExtensiblePropertyContainer container )
        {
            return GetValueFrom( container, default( T ) );
        }

        /// <summary>Gets a value for the property from the container</summary>
        /// <param name="container">container</param>
        /// <param name="defaultValue">default value if the value is not yet present as an extended property</param>
        /// <returns>Value retrieved from the property or <paramref name="defaultValue"/> if it wasn't found</returns>
        /// <remarks>If the value didn't exist a new value with <paramref name="defaultValue"/> is added to the container</remarks>
        public T GetValueFrom( IExtensiblePropertyContainer container, T defaultValue )
        {
            return GetValueFrom( container, ( ) => defaultValue );
        }

        /// <summary>Gets a value for the property from the container</summary>
        /// <param name="container">container</param>
        /// <param name="lazyDefaultFactory">default value factory delegate to create the default value if the value is not yet present as an extended property</param>
        /// <returns>Value retrieved from the property or default value created by <paramref name="lazyDefaultFactory"/> if it wasn't found</returns>
        /// <remarks>If the value didn't exist a new value created by calling with <paramref name="lazyDefaultFactory"/> is added to the container</remarks>
        public T GetValueFrom( IExtensiblePropertyContainer container, Func<T> lazyDefaultFactory )
        {
            if( container == null )
            {
                throw new ArgumentNullException( nameof( container ) );
            }

            if( lazyDefaultFactory == null )
            {
                throw new ArgumentNullException( nameof( lazyDefaultFactory ) );
            }

            if( container.TryGetExtendedPropertyValue( Name, out T retVal ) )
            {
                return retVal;
            }

            retVal = lazyDefaultFactory( );
            container.AddExtendedPropertyValue( Name, retVal );
            return retVal;
        }

        /// <summary>Sets the value of an extended property in a container</summary>
        /// <param name="container">Container to set the value in</param>
        /// <param name="value">value of the property</param>
        public void SetValueIn( IExtensiblePropertyContainer container, T value )
        {
            if( container == null )
            {
                throw new ArgumentNullException( nameof( container ) );
            }

            container.AddExtendedPropertyValue( Name, value );
        }

        /// <summary>Name of the property</summary>
        public string Name { get; }
    }
}
