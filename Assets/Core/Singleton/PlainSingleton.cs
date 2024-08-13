using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Payosky.Core.Singleton
{
    public class PlainSingleton<T> where T : class, new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                instance ??= new T();
                return instance;
            }
        }

        protected PlainSingleton() { }

    }//Closes PlainSingleton class
}//Closes Namespace declaration
