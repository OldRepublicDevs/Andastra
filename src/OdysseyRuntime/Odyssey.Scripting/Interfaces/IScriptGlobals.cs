using Odyssey.Core.Interfaces;

namespace Odyssey.Scripting.Interfaces
{
    /// <summary>
    /// Persistent script state - global and local variables.
    /// </summary>
    public interface IScriptGlobals
    {
        // Global integers
        int GetGlobalInt(string name);
        void SetGlobalInt(string name, int value);
        
        // Global booleans
        bool GetGlobalBool(string name);
        void SetGlobalBool(string name, bool value);
        
        // Global strings
        string GetGlobalString(string name);
        void SetGlobalString(string name, string value);
        
        // Global locations
        object GetGlobalLocation(string name);
        void SetGlobalLocation(string name, object value);
        
        // Local integers (per object)
        int GetLocalInt(IEntity entity, string name);
        void SetLocalInt(IEntity entity, string name, int value);
        
        // Local floats (per object)
        float GetLocalFloat(IEntity entity, string name);
        void SetLocalFloat(IEntity entity, string name, float value);
        
        // Local strings (per object)
        string GetLocalString(IEntity entity, string name);
        void SetLocalString(IEntity entity, string name, string value);
        
        // Local objects (per object)
        IEntity GetLocalObject(IEntity entity, string name);
        void SetLocalObject(IEntity entity, string name, IEntity value);
        
        // Local locations (per object)
        object GetLocalLocation(IEntity entity, string name);
        void SetLocalLocation(IEntity entity, string name, object value);
        
        /// <summary>
        /// Clears all local variables for an entity.
        /// </summary>
        void ClearLocals(IEntity entity);
        
        /// <summary>
        /// Clears all global variables.
        /// </summary>
        void ClearGlobals();
    }
}

