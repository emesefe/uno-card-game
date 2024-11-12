```
  _   _ _   _  ___     ____    _    ____  ____     ____    _    __  __ _____ 
 | | | | \ | |/ _ \   / ___|  / \  |  _ \|  _ \   / ___|  / \  |  \/  | ____|
 | | | |  \| | | | | | |     / _ \ | |_) | | | | | |  _  / _ \ | |\/| |  _|  
 | |_| | |\  | |_| | | |___ / ___ \|  _ <| |_| | | |_| |/ ___ \| |  | | |___ 
  \___/|_| \_|\___/   \____/_/   \_\_| \_\____/   \____/_/   \_\_|  |_|_____|
                                                                             
```

# Cartas

<details>
<summary>Totales ☑️</summary>
<br>
    <ul>
        <li>- [x] Hay 4 colores (BLUE, YELLOW, RED, GREEN por defecto, otros 4 especiales para daltónicos)</li>
        <li>- [x] Los tipos de cartas son NUMBER, SKIP, INVERT, PLUS2, PLUS4, CHANGE_COLOR</li>
        <li>- [x] Hay un total de 80 cartas NUMBER (2 por cada color y símbolo, del 0 al 9)</li>
        <li>- [x] Hay un total de 8 cartas SKIP (2 por cada color)</li>
        <li>- [x] Hay un total de 8 cartas INVERT (2 por cada color)</li>
        <li>- [x] Hay un total de 8 cartas PLUS2 (2 por cada color)</li>
        <li>- [x] Hay un total de 4 cartas PLUS4</li>
        <li>- [x] Hay un total de 4 cartas CHANGE_COLOR</li>
        <li>- [x] Hay un total de 112 cartas</li>
    </ul>
</details>

<details>
<summary>Scriptable Object (SOCard) ☑️</summary>
<br>
    <ul>
        <li>- [x] Type</li>
        <li>- [x] Digit</li>
        <li>- [x] Sprite</li>
    </ul>
</details>

<details>
<summary>Visual ☑️</summary>
<br>
    <ul>
        <li>- [x] Color</li>
        <li>- [x] Background de la carta con un color</li>
        <li>- [x] Sprite (del SO Card)</li>
    </ul>
</details>

<details>
<summary>Lógica</summary>
<br>
    <ul>
        <li>- [x] SKIP -> Saltar turno</li>
        <li>- [x] INVERT -> Cambiar sentido de turnos</li>
        <li>- [ ] PLUS2 -> Robar carta y saltar turno</li>
        <li>- [ ] PLUS4 -> Cambiar de color, robar carta y saltar turno</li>
        <li>- [ ] CHANGE_COLOR -> Cambiar de color</li>
    </ul>
</details>

# Estructura del juego 

<details>
<summary>General</summary>
<br>
    <ul>
        <li>- [x] Los jugadores se guardarán en un array</li>
        <li>- [ ] Mínimo de 2 jugadores</li>
        <li>- [ ] Máximo de 10 jugadores</li>
        <li>- [x] El turno será un enum</li>
        <li>- [x] El sentido de los turnos será un bool</li>
        <li>- [x] El mazo de descarte será una list</li>
        <li>- [x] El mazo de robo será una list</li>
        <li>- [x] La mano del jugador será una list</li>
    </ul>
</details>

## Escenas

<details>
<summary>Main Menu</summary>
<br>
    <ul>
        <li>- [ ] Botón Play -> Select Username</li>
        <li>- [ ] Botón Options -> Options</li>
        <li>- [ ] Botón Exit</li>
    </ul>
</details>

<details>
<summary>Options</summary>
<br>
    <ul>  
        <li>- [ ] Modificar volumen música</li>
        <li>- [ ] Modificar volumen SFX</li>
        <li>- [ ] Modificar idioma</li>
        <li>- [ ] Elegir colores (2 opciones: clásica o daltónicos)</li>
        <li>- [ ] Elegir duración timer (opciones preconfiguradas)</li>
        <li>- [ ] Elegir cantidad de mazos (1 o 2)</li>
    </ul>
</details>

<details>
<summary>Select Username</summary>
<br>
    <ul>  
        <li>- [ ] Introducir username. Si no se introduce nada, por defecto es Guest</li>
        <li>- [ ] Botón continue -> Game</li>
    </ul>
</details>

<details>
<summary>Game</summary>
<br>
    <ul>  
        <li>- [ ] Panel Win con 2 botones (volver a jugar o volver a Main Menu). Mostrar total de rondas ganadas en la misma sesión.</li>
        <li>- [ ] Panel Game Over con 2 botones (volver a jugar o volver a Main Menu). Mostrar total de rondas ganadas en la misma sesión.</li>
        <li>- [x] Panel de selector de color (4 opciones de color y opción random)</li>
        <li>- [ ] Añadir a panel de selector de color la opción random</li>
        <li>- [ ] Mostrar de qué jugador es el turno haciendo que el icono del jugador esté destacado (más grande, brillo, indicador sobre el icono)</li>
        <li>- [x] Mostrar la mano del jugador</li>
        <li>- [ ] Si no es el turno del jugador, las cartas se ven más pequeñas e incluso más oscuras</li>
        <li>- [x] Ajustar las cartas de la mano del jugador al espacio disponible y en función de la cantidad</li>
        <li>- [x] Botón de confirmar jugada</li>
        <li>- [x] Mostrar el mazo de robo</li>
        <li>- [ ] Botón de robar carta</li>
        <li>- [ ] Mostrar el mazo de descarte con un mínimo de 4 cartas</li>
        <li>- [ ] Mostrar el sentido del juego (horario o antihorario)</li>
        <li>- [ ] Cuando el sentido cambia, destacar el icono que representa el sentido del juego</li>
        <li>- [ ] Mostrar, cuando se juegue un PLUS2 o un PLUS4 un acumulador con el total de cartas a robar</li>
        <li>- [ ] Mostrar el temporizador del juego</li>
        <li>- [ ] Mostrar icono del resto de jugadores con username y el total de cartas que tienen en su manos</li>
    </ul>
</details>

# Acciones del juego

<details>
<summary>General</summary>
<br>
    <ul>  
        <li>- [ ] Validar username</li>
        <li>- [ ] El primer turno es al azar</li>
        <li>- [x] Cambiar de turno, teninendo en cuenta si se salta a un jugador o varios</li>
        <li>- [x] Perder turno / Saltar turno</li>
        <li>- [ ] En cada turno conocemos la última carta en el mazo de descarte (símbolo y color), el jugador que tiene el turno y el tiempo restante del turno</li>
        <li>- [ ] Reiniciar temporizador</li>
        <li>- [ ] Detectar si el temporizador ha finalizado</li>
        <li>- [ ] Cambiar de color</li>
        <li>- [ ] El turno de un jugador finaliza cuando ha hecho una jugada, ha robado o se ha acabado el turno</li>
        <li>- [ ] Si se acaba el turno, roba automáticamente 2 cartas y pierde el turno</li>
        <li>- [x] Añadir carta a mazo de robo</li>
        <li>- [x] Generar el mazo de robo</li>
        <li>- [x] Mezclar cartas</li>
        <li>- [x] Robar una carta consiste en coger la última carta del mazo de robo y devolver dicha carta</li>
        <li>- [x] Añadir carta a mazo de descarte</li>
        <li>- [ ] Convertir mazo de descarte a mazo de robo. Se cambian todas las cartas de mazo salvo la última del mazo de descarte (la última jugada)</li>
        <li>- [x] Iniciar mano del jugador. Los jugadores empiezan con 7 cartas</li>
        <li>- [x] Añadir carta a la mano del jugador</li>
        <li>- [x] Eliminar carta de la mano del jugador</li>
        <li>- [x] Jugar una carta. En tu turno puedes jugar una o varias cartas (si son exactamente iguales en símbolo y color)</li>
        <li>- [ ] Si el jugador es IA, utilizar corrutinas para que la jugada no sea immediata</li>
        <li>- [x] El jugador selecciona la carta o cartas a tirar con click izquierdo</li>
        <li>- [x] Validar si una carta se puede jugar. Se puede jugar una carta por símbolo (número o carta especial) o color</li>
        <li>- [x] Seleccionar una carta de la mano del jugador</li>
        <li>- [x] Si una carta está seleccionada y se le da un nuevo click izquierdo, vuelve a su estado original</li>
        <li>- [x] Si la carta seleccionada para la jugada es una carta válida, se agranda en la mano para mostrar visualmente que se puede jugar y está seleccionada para ser jugada</li>
        <li>- [x] Si la carta seleccionada para la jugada es una carta válida, se mantiene elevada sobre el resto y está seleccionada para ser jugada</li>
        <li>- [x] Si la carta seleccionada no es válida para la jugada (teniendo en cuenta las cartas previamente seleccionadas), vuelve a su estado original (recupera su escala)</li>
        <li>- [x] Si la carta seleccionada no es válida para la jugada (teniendo en cuenta las cartas previamente seleccionadas), vuelve a su posición original en la mano</li>
        <li>- [ ] Si la carta seleccionada no es válida para la jugada (teniendo en cuenta las cartas previamente seleccionadas), suena un SFX</li>
        <li>- [x] El jugador confirma la jugada con un botón adicional en la UI (deberes para vosotros)</li>
        <li>- [x] Si se hace click sobre el botón de confirmación de la jugada, se juegan todas las cartas seleccionadas</li>
        <li>- [x] En caso de no poder tirar una carta, se roba hasta tener una válida y es obligatorio jugarla</li>
        <li>- [x] El jugador solamente puede robar si no tiene ninguna carta que poder jugar</li>
        <li>- [ ] Si el jugador quiere robar teniendo al menos una carta que poder jugar, sonará un SFX y se destacarán las cartas que se pueden jugar (deberes para vosotros)</li>
        <li>- [ ] En caso de que a un jugador le quede una sola carta, aparecerá un botón en un punto aleatorio de la pantalla, sonará un SFX y el jugador con una carta tiene que ser el primero en darle al botón. Si otro jugador es más rápido en darle al botón, el jugador al que le quedaba una carta deberá robar dos cartas y pierde el turno</li>
        <li>- [ ] En el mazo de descarte se pueden acumular solamente PLUS2 infinitamente. Es decir, después de un PLUS2 el siguiente jugador puede tirar otro PLUS2 y librarse de robar, pero no puede tirar un PLUS4</li>
        <li>- [ ] Si al jugador le tiran un PLUS2 y dispone de un PLUS2, tiene que tirarlo, no puede guardárselo</li>
        <li>- [ ] En el mazo de descarte se pueden acumular solamente PLUS4 infinitamente. Es decir, después de un PLUS4 el siguiente jugador puede tirar otro PLUS4 y librarse de robar, pero no puede tirar un PLUS2</li>
        <li>- [ ] Si al jugador le tiran un PLUS4 y dispone de un PLUS4, tiene que tirarlo, no puede guardárselo</li>
        <li>- [ ] Si se salta un turno a un jugador por una carta SKIP, dicho jugador tendrá un icono de prohíbido sobre su icono de jugador. Si se trata del propio jugador el que está bloqueado, el botón de confirmar tendrá un icono de prohibido o estará deshabilitado</li>
        <li>- [ ] Configuar el volumen de la música</li>
        <li>- [ ] Configuar el volumen de los SFX</li>
        <li>- [ ] Configurar el tamaño de la ventana de juego</li>
        <li>- [ ] Configurar el idioma</li>
        <li>- [ ] Configurar los colores del mazo con opciones preconfiguradas (clásica y daltónicos)</li>
        <li>- [ ] Configurar la duración del temporizador con opciones preconfiguradas</li>
        <li>- [ ] Configurar la cantidad de mazos con opciones preconfiguradas (1 o 2)</li>
    </ul>
</details>