using UnityEngine;

/// <summary>Algo que se puede atacar: piezas y cajas.</summary>
public interface IAttackable
{
    /// <summary>Se llama al recibir un ataque.</summary>
    void Attacked(); // Se llama cuando la entidad es atacada
}
