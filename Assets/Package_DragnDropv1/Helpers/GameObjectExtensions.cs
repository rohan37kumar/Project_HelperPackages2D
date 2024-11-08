
using UnityEngine;
namespace Utilitis
{
   
        public static class GameObjectExtensions
        {
            public static T GetOrAddComponent<T>(this GameObject gameObject)where T : Component
            {
                T component = gameObject.GetComponent<T>();
                return component != null ? component : gameObject.AddComponent<T>();
            }
        }
    
    

}