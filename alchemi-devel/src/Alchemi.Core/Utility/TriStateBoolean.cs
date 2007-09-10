#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   TriStateBoolean.cs
 * Project      :   Alchemi.Core.Utility
 * Created on   :   08 November 2005
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Tibor Biro (tb@tbiro.com)
 * License      :   GPL
 *                  This program is free software; you can redistribute it and/or 
 *                  modify it under the terms of the GNU General Public
 *                  License as published by the Free Software Foundation;
 *                  See the GNU General Public License 
 *                  (http://www.gnu.org/copyleft/gpl.html) for more details.
 *
 */
#endregion

using System;

namespace Alchemi.Core.Utility
{
	/// <summary>
	/// Define a structure that holds a tri state boolean name: true, false, undefined
	/// </summary>
	public struct TriStateBoolean
	{
        public static readonly TriStateBoolean True = new TriStateBoolean(1);
        public static readonly TriStateBoolean False = new TriStateBoolean(0);
        public static readonly TriStateBoolean Undefined = new TriStateBoolean(-1);

		private int m_value;


        #region Private Constructor
        private TriStateBoolean(int value)
        {
            m_value = value;
        } 
        #endregion

		

		// NOTE: Equals must be overridden for Value types because of the inefficient default implementation.

        #region Method Override - Equals
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is TriStateBoolean))
            {
                return false;
            }

            return this.Equals((TriStateBoolean)obj);
        } 
        #endregion


        #region Method - Equals
        public bool Equals(TriStateBoolean obj)
        {
            return obj.m_value.Equals(m_value);
        } 
        #endregion



        #region Method Override - GetHashCode
        public override int GetHashCode()
        {
            return m_value.GetHashCode();
        } 
        #endregion


        #region Method Override - ToString
        public override string ToString()
        {
            return m_value.ToString();
        } 
        #endregion



        #region == Operator
        public static Boolean operator ==(TriStateBoolean obj1, TriStateBoolean obj2)
        {
            return (obj1.Equals(obj2));
        } 
        #endregion


        #region != Operator
        public static Boolean operator !=(TriStateBoolean obj1, TriStateBoolean obj2)
        {
            return !(obj1 == obj2);
        } 
        #endregion

	}
}
