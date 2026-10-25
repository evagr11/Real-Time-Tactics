<div align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0%3A00ebc5%2C100%3Aff2d85&height=200&section=header&text=Real+Time+Tactics&fontSize=60&fontColor=fff&fontAlignY=38&desc=Un+juego+de+estrategia+t%C3%A1ctica+1v1+en+tiempo+real&descAlignY=62&descSize=18" width="100%"/>
</div>

<div align="center">
  <a href="https://github.com/evagr11/Real-Time-Tactics" target="_blank"><img src="https://img.shields.io/badge/Ver_Código-181717?style=for-the-badge&logo=github&logoColor=white" alt="Ver Código"/></a>
  <a href="https://evagr11.github.io/Curriculum/proyectos/real-time-tactics.html" target="_blank"><img src="https://img.shields.io/badge/Ver_Web-00b89a?style=for-the-badge&logo=googlechrome&logoColor=white" alt="Ver Web"/></a>
  <a href="https://evagr.itch.io/real-time-tactics" target="_blank"><img src="https://img.shields.io/badge/Jugar_en_itch.io-FA5C5C?style=for-the-badge&logo=itch.io&logoColor=white" alt="Jugar en itch.io"/></a>
</div>

<br>

<div align="center">
  <a href="https://git.io/typing-svg"><img src="https://readme-typing-svg.demolab.com/?font=JetBrains+Mono&size=18&duration=3500&pause=800&color=ff2d85&center=true&vCenter=true&width=600&lines=Desarrollado+en+Unity+3D%3BGameplay+t%C3%A1ctico+1v1%3BTurnos+limitados+en+tiempo+real%3BTablero+5x5+con+cursor+toroidal%3BScripts+en+C%23%3BMec%C3%A1nica+de+catch-up" alt="Typing SVG" /></a>
</div>

# 🚀 Sobre el Proyecto

**Real-Time Tactics (RTT)** es un juego de estrategia táctica **1v1** desarrollado en **Unity 3D**. La partida se desarrolla en un tablero de **5x5** donde el **cursor es toroidal**: si sale por un lado, aparece por el opuesto, así que ninguna casilla queda lejos. El escenario es un bosque encantado nocturno, con una atmósfera inmersiva creada a través de modelos 3D, efectos de partículas y una cuidada iluminación. Cada jugador controla **3 piezas con 3 puntos de vida** y gana quien elimine las del oponente.

<br>

<div align="center">
  <img src="https://github.com/user-attachments/assets/745d3b77-5773-4764-b233-632d07e0977b" alt="Mockup del juego" width="70%"/>
  <p><em>Mockup del juego</em></p>
</div>


### ✨ Características principales:

*   👑 **Estrategia 1v1**: Enfréntate a otro jugador en un duelo táctico por la supremacía del tablero.
*   ⏱️ **Turnos en Tiempo Real**: Cada turno dura **15 segundos**, marcados por una barra del color del jugador. En ese tiempo puedes hacer todo lo que te dé tiempo; quién empieza se decide al azar.
*   🌐 **Cursor Toroidal**: El cursor no tiene bordes y llega a cualquier pieza en pocos pasos; las piezas, en cambio, solo avanzan a casillas vecinas, así que la posición importa.
*   ❤️ **Gestión de Salud Visual**: La altura de las piezas representa su vida. A medida que reciben daño, se encogen visualmente.
*   ⚔️ **Ataque en cruz con enfriamiento**: Una pieza golpea a la vez las cuatro casillas vecinas y después espera unos segundos antes de volver a atacar.
*   ⚡ **Mecánica de Remontada (Catch-up)**: El enfriamiento base es de 2 s y se multiplica por las piezas que te quedan entre las iniciales (con un mínimo del 20 %): con una sola pieza atacas el triple de rápido.
*   📦 **Cajas y corazones**: Cada 5-15 segundos aparece una caja (máximo 3). Al romperla deja un corazón que cura 1 punto de vida a la pieza que lo recoge.
*   🌲 **Ambientación Inmersiva**: Sumérgete en un bosque mágico con modelos 3D, efectos de partículas y una atmósfera nocturna.

### 🎮 Cómo se juega

Los dos jugadores comparten teclado y un mismo cursor, que cambia de color según de quién sea el turno.

<div align="center">

| Acción | Jugador 1 | Jugador 2 |
| :----- | :-------: | :-------: |
| Mover el cursor / la pieza | `W` `A` `S` `D` | Flechas |
| Coger / soltar una pieza | `Ctrl izquierdo` | `Ctrl derecho` |
| Atacar (con la pieza cogida) | `Espacio` | `Intro` |
| Volver a jugar (en la pantalla de victoria) | `Espacio` | `Espacio` |

</div>

1. Lleva el cursor a una de tus piezas y cógela.
2. Muévela a una casilla vecina libre (o con un corazón) y suéltala, o ataca desde donde está.
3. Si se acaba tu turno con una pieza cogida, se suelta sola.

# 🧠 Contexto del Proyecto

Este proyecto fue desarrollado como parte de la asignatura de **Programación** durante el primer año del ciclo formativo de **Desarrollo de Aplicaciones Multiplataforma (DAM)**. Sirvió como una aplicación práctica de los conceptos de programación orientada a objetos, gestión de estados y desarrollo de videojuegos con el motor Unity.

# 🛠️ Stack Tecnológico
<div align="center">
  <img src="https://img.shields.io/badge/Unity-000000?style=for-the-badge&logo=unity&logoColor=white" alt="Unity"/>
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C#"/>
  <img src="https://img.shields.io/badge/Status-Stable-brightgreen?style=for-the-badge" alt="Status"/>
</div>

<br>

<div align="center">

| Tecnología | Descripción |
| :--------- | :---------- |
| **Motor** | `Unity 2023.2` (3D) |
| **Lenguaje** | `C#` |
| **Arquitectura** | Modelo / Vista / Controlador, interfaces (`IGameEntity`, `IAttackable`) y eventos (`GameEvents`) |
| **Visual** | Modelos 3D (`.obj`), materiales propios, sistemas de partículas y corrutinas para las animaciones |
| **Interfaz** | `Unity UI` (barra de tiempo) y escenas de victoria |
| **Entorno** | `Visual Studio` |

</div>

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
│   │   │   ├── Board.cs           # Tablero, casillas y ataques
│   │   │   ├── BoardEntity.cs     # Base de todo lo que ocupa una casilla
│   │   │   ├── Piece.cs           # Vida, daño, curación y enfriamiento
│   │   │   ├── Crate.cs           # Caja que suelta un corazón al romperse
│   │   │   ├── HealthPickup.cs    # Corazón que cura
│   │   │   ├── EntityID.cs
│   │   │   ├── IAttackable.cs
│   │   │   └── IGameEntity.cs
│   │   ├── View/
│   │   │   ├── CrateVisuals.cs
│   │   │   ├── CursorVisual.cs
│   │   │   ├── PieceVisual.cs     # Encoger al recibir daño, partículas y enfriamiento
│   │   │   └── HealthPickupVisual.cs
│   │   └── ResetGame.cs           # Reinicia desde la pantalla de victoria
│   ├── Scenes/
│   │   ├── Principal.unity        # Partida
│   │   ├── Win1.unity             # Gana el jugador 1
│   │   └── Win2.unity             # Gana el jugador 2
│   ├── Prefabs/                   # Piezas, caja, corazón, cursor, casilla y partículas
│   ├── Materiales/
│   ├── ArchivoBox/                # Modelo 3D de la caja
│   ├── ArchivoHeart/              # Modelo 3D del corazón
│   └── ArchivoPlantas/            # Modelos 3D del bosque
├── Packages/
│   └── manifest.json
└── ProjectSettings/
```

# 🚀 Cómo Utilizar el Proyecto

Para ejecutar este proyecto localmente necesitas **Unity Hub** y **Unity 2023.2** (se creó con la 2023.2.20f1). Si solo quieres jugar, está [en itch.io](https://evagr.itch.io/real-time-tactics).

1.  **Clona el repositorio:**
    ```bash
    git clone https://github.com/evagr11/Real-Time-Tactics.git
    ```

2.  **Abre el proyecto en Unity:**
    *   Abre **Unity Hub**.
    *   Haz clic en `Open` o `Abrir`.
    *   Navega hasta la carpeta del proyecto clonado y selecciónala. Unity se encargará de importar y configurar el proyecto.

3.  **Ejecuta el juego:**
    *   Una vez que el proyecto esté abierto en el Editor de Unity, abre la escena `Principal` de la carpeta `Assets/Scenes`.
    *   Presiona el botón de **Play** ▶️ en la parte superior central del editor para iniciar el juego.

<br>

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
