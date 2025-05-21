using UnityEngine;

public interface IAttackable
{
    // Interfaz que indica que una entidad puede ser atacada (recibir daño o ser destruida).    
    void Attacked(); // Método que se llama cuando la entidad es atacada
}