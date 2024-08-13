using UnityEngine;


namespace Alternova.Runtime
{
    [CreateAssetMenu(fileName = "new JSON Loader", menuName = "ScriptableObjects/JSON Loader", order = 1)]

    public class JsonDataLoader : ScriptableObject
    {


        public TextAsset jsonFile;

        public T LoadData<T>() where T : class
        {

            if (jsonFile == null) return null;

            try
            {
                T data = JsonUtility.FromJson<T>(jsonFile.text);
                return data;
            }
            catch (System.Exception e)
            {


                return null;
            }
        }//Closes DataLoaderResult method


    }//Closes JsonDataLoader scriptableobject
}//Closes namespace declaration