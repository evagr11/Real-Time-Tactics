<div align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0%3A00ebc5%2C100%3Aff2d85&height=200&section=header&text=Real+Time+Tactics&fontSize=60&fontColor=fff&fontAlignY=38&desc=Un+juego+de+estrategia+t%C3%A1ctica+1v1+en+tiempo+real&descAlignY=62&descSize=18" width="100%"/>
</div>

<div align="center">
  <img src="https://img.shields.io/badge/Ver_Código-gray?style=for-the-badge&logo=github" alt="logo de github" onclick="window.open('https://github.com/evagr11/Real-Time-Tactics', '_blank')" style="cursor: pointer;"/>
  <img src="https://img.shields.io/badge/Ver_Web-blue?style=for-the-badge&logo=googlechrome&logoColor=white" alt="logo de web" onclick="window.open('https://evagr11.github.io/Curriculum/proyectos/real-time-tactics.html', '_blank')" style="cursor: pointer;"/>
  <img src="https://img.shields.io/badge/Ver_en_itch.io-FA5C5C?style=for-the-badge&logo=itch.io" alt="logo de itch.io" onclick="window.open('https://evagr.itch.io/real-time-tactics', '_blank')" style="cursor: pointer;"/>
</div>

  <br>

<div align="center">
  <a href="https://git.io/typing-svg"><img src="https://readme-typing-svg.demolab.com/?font=JetBrains+Mono&size=18&duration=3500&pause=800&color=ffffff&center=true&vCenter=true&width=600&lines=Desarrollado+en+Unity+3D%3BGameplay+t%C3%A1ctico+1v1%3BTurnos+limitados+en+tiempo+real%3BTablero+toroidal+5x5%3BScripts+en+C%23%3BMec%C3%A1nica+de+catch-up" alt="Typing SVG" /></a>
</div>

# 🚀 Sobre el Proyecto

**Real-Time Tactics (RTT)** es un juego de estrategia táctica **1v1** desarrollado en **Unity 3D**. La partida se desarrolla en un tablero de **5x5 con geometría toroidal**, lo que significa que si una pieza sale por un lado, aparece por el opuesto. El escenario es un bosque encantado nocturno, con una atmósfera inmersiva creada a través de modelos 3D, efectos de partículas y una cuidada iluminación. Cada jugador controla 3 piezas cuyo objetivo es eliminar las del oponente.

<br>

<div align="center">
  <img src="https://github.com/user-attachments/assets/745d3b77-5773-4764-b233-632d07e0977b" alt="Mockup del juego" width="70%"/>
  <p><em>Mockup del juego</em></p>
</div>


### ✨ Características principales:

*   👑 **Estrategia 1v1**: Enfréntate a otro jugador en un duelo táctico por la supremacía del tablero.
*   ⏱️ **Turnos en Tiempo Real**: Cada jugador dispone de un tiempo limitado para ejecutar movimientos, ataques y curaciones antes de ceder el turno al oponente.
*   🌐 **Tablero Toroidal**: Un campo de batalla 5x5 sin bordes, donde los flancos no existen y la estrategia posicional es clave.
*   ❤️ **Gestión de Salud Visual**: La altura de las piezas representa su vida. A medida que reciben daño, se encogen visualmente.
*   ⚡ **Mecánica de Remontada (Catch-up)**: Si pierdes piezas, tus tiempos de enfriamiento (cooldowns) se reducen, dándote una oportunidad para cambiar el rumbo de la partida.
*   🌲 **Ambientación Inmersiva**: Sumérgete en un bosque mágico con modelos 3D, efectos de partículas y una atmósfera nocturna.

# 🧠 Contexto del Proyecto

Este proyecto fue desarrollado como parte de la asignatura de **Programación** durante el primer año del ciclo formativo de **Desarrollo de Aplicaciones Multiplataforma (DAM)**. Sirvió como una aplicación práctica de los conceptos de programación orientada a objetos, gestión de estados y desarrollo de videojuegos con el motor Unity.

# 🛠️ Stack Tecnológico
<div align="center">
  <img src="https://img.shields.io/badge/Unity-000000?style=for-the-badge&logo=unity&logoColor=white" alt="Unity"/>
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C#"/>
  <img src="https://img.shields.io/badge/Status-Stable-brightgreen?style=for-the-badge" alt="Status"/>
</div>

| Tecnología | Descripción                                          |
| :--------- | :--------------------------------------------------- |
| **Engine** | `Unity 3D` - Motor de juego para el desarrollo 2D/3D. |
| **Lenguaje** | `C#` - Lenguaje principal para la lógica del juego. |
| **Entorno**  | `Visual Studio` - IDE para la escritura de scripts.  |

# 🖼️ Capturas del Juego

| Inicio de la Partida                                         | Un jugador se cura                                      |
| :----------------------------------------------------------: | :------------------------------------------------------: |
| <img src="https://github.com/user-attachments/assets/4f0fce38-4433-4e03-afbe-b01aae22a52c" alt="Imagen de una partida" width="100%"/> | <img src="https://github.com/user-attachments/assets/75d3cbea-5e00-4b9f-b818-f0dc57d0fbb0" alt="Jugador se cura" width="100%"/> |
| *Una partida recién comenzada.*                              | *Una pieza recoge un corazón para recuperar vida.*         |

<br>

<div align="center">
  <img src="https://github.com/user-attachments/assets/bf46613c-2ee7-4498-b3db-7730c2d868c1" alt="Imagen de la partida cuando un jugador gana" width="60%"/>
  <p><em>Pantalla que se muestra al ganar la partida.</em></p>
</div>

# 📂 Estructura del Proyecto

La estructura del proyecto sigue las convenciones de Unity, separando la lógica (`Controller`), los datos (`Model`) y la representación visual (`View`).

```bash
Real-Time-Tactics/
├── Assets/
│   ├── Scripts/
│   │   ├── Controller/
│   │   │   ├── CursorLogic.cs
│   │   │   ├── GameEvents.cs
│   │   │   ├── GameManager.cs
│   │   │   └── Square.cs
│   │   ├── Model/
│   │   │   ├── Board.cs
│   │   │   ├── Piece.cs
│   │   │   ├── Crate.cs
│   │   │   └── IGameEntity.cs
│   │   └── View/
│   │       ├── CrateVisuals.cs
│   │       ├── CursorVisual.cs
│   │       ├── PieceVisual.cs
│   │       └── HealthPickupVisual.cs
│   ├── ArchivoBox/
│   ├── ArchivoHeart/
│   └── ArchivoPlantas/
├── Packages/
│   └── manifest.json
└── ProjectSettings/
```

# 🚀 Cómo Utilizar el Proyecto

Para ejecutar este proyecto localmente, necesitarás tener instalado Unity Hub y una versión compatible del Editor de Unity.

1.  **Clona el repositorio:**
    ```bash
    git clone https://github.com/evagr11/Real-Time-Tactics.git
    ```

2.  **Abre el proyecto en Unity:**
    *   Abre **Unity Hub**.
    *   Haz clic en `Open` o `Abrir`.
    *   Navega hasta la carpeta del proyecto clonado y selecciónala. Unity se encargará de importar y configurar el proyecto.

3.  **Ejecuta el juego:**
    *   Una vez que el proyecto esté abierto en el Editor de Unity, busca la escena principal llamada `Principal` en la carpeta `Assets/Scenes` (o en la raíz de `Assets`).
    *   Haz doble clic sobre ella para abrirla.
    *   Presiona el botón de **Play** ▶️ en la parte superior central del editor para iniciar el juego.

</br>

<div align="center">
  <p>Creado por <strong>Eva Gallardo</strong></p>
  <a href="https://github.com/evagr11" target="_blank">
    <img src="https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white" alt="GitHub">
  </a>
  <a href="https://www.linkedin.com/in/eva-gallardo-romero-830362349/" target="_blank">
    <img src="https://img.shields.io/badge/LinkedIn-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white" alt="LinkedIn">
  </a>
</div>

<div align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0%3A00ebc5%2C100%3Aff2d85&height=120&section=footer" width="100%"/>
</div>
