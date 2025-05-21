using UnityEngine;

public interface IAttackable
{
    // Interfaz que indica que una entidad puede ser atacada (recibir da�o o ser destruida).    
    void Attacked(); // M�todo que se llama cuando la entidad es atacada
}