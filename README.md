```
  _   _ _   _  ___     ____    _    ____  ____     ____    _    __  __ _____ 
 | | | | \ | |/ _ \   / ___|  / \  |  _ \|  _ \   / ___|  / \  |  \/  | ____|
 | | | |  \| | | | | | |     / _ \ | |_) | | | | | |  _  / _ \ | |\/| |  _|  
 | |_| | |\  | |_| | | |___ / ___ \|  _ <| |_| | | |_| |/ ___ \| |  | | |___ 
  \___/|_| \_|\___/   \____/_/   \_\_| \_\____/   \____/_/   \_\_|  |_|_____|
                                                                             
```

# Cartas

- [x] Hay 4 colores (BLUE, YELLOW, RED, GREEN por defecto, otros 4 especiales para daltónicos)
- [x] Los tipos de cartas son NUMBER, SKIP, INVERT, PLUS2, PLUS4, CHANGE_COLOR
- [x] Hay un total de 80 cartas NUMBER (2 por cada color y símbolo, del 0 al 9)
- [x] Hay un total de 8 cartas SKIP (2 por cada color)
- [x] Hay un total de 8 cartas INVERT (2 por cada color)
- [x] Hay un total de 8 cartas PLUS2 (2 por cada color)
- [x] Hay un total de 4 cartas PLUS4
- [x] Hay un total de 4 cartas CHANGE_COLOR
- [x] Hay un total de 112 cartas

### Scriptable Object (SOCard)

- Type
- Digit
- Sprite

### Visual

- [x] Color
- [x] Background de la carta con un color
- [x] Sprite (del SO Card)

### Lógica

- SKIP -> Saltar turno
- INVERT -> Cambiar sentido de turnos
- PLUS2 -> Robar carta y saltar turno
- PLUS4 -> Cambiar de color, robar carta y saltar turno
- CHANGE_COLOR -> Cambiar de color

# Estructura del juego 

- [ ] Los jugadores se guardarán en un array
- [ ] Mínimo de 2 jugadores
- [ ] Máximo de 10 jugadores

- [ ] El turno será un enum
- [ ] El sentido de los turnos será un bool

- [ ] El mazo de descarte será una list
- [x] El mazo de robo será una list

- [ ] La mano del jugador será una list

## Escenas

### Main Menu

- [ ] Botón Play -> Select Username
- [ ] Botón Options -> Options
- [ ] Botón Exit

### Options

- [ ] Modificar volumen música
- [ ] Modificar volumen SFX
- [ ] Modificar idioma
- [ ] Elegir colores (2 opciones: clásica o daltónicos)
- [ ] Elegir duración timer (opciones preconfiguradas)
- [ ] Elegir cantidad de mazos (1 o 2)

### Select Username

- [ ] Introducir username. Si no se introduce nada, por defecto es Guest
- [ ] Botón continue -> Game

### Game

- [ ] Panel Win con 2 botones (volver a jugar o volver a Main Menu). Mostrar total de rondas ganadas en la misma sesión.
- [ ] Panel Game Over con 2 botones (volver a jugar o volver a Main Menu). Mostrar total de rondas ganadas en la misma sesión.
- [ ] Panel de selector de color (4 opciones de color y opción random)
- [ ] Mostrar de qué jugador es el turno haciendo que el icono del jugador esté destacado (más grande, brillo, indicador sobre el icono)
- [ ] Mostrar la mano del jugador
- [ ] Si no es el turno del jugador, las cartas se ven más pequeñas e incluso más oscuras
- [ ] Ajustar las cartas de la mano del jugador al espacio disponible y en función de la cantidad
- [ ] Botón de confirmar jugada
- [ ] Mostrar el mazo de robo
- [ ] Botón de robar carta
- [ ] Mostrar el mazo de descarte con un mínimo de 4 cartas
- [ ] Mostrar el sentido del juego (horario o antihorario)
- [ ] Cuando el sentido cambia, destacar el icono que representa el sentido del juego
- [ ] Mostrar, cuando se juegue un PLUS2 o un PLUS4 un acumulador con el total de cartas a robar
- [ ] Mostrar el temporizador del juego
- [ ] Mostrar icono del resto de jugadores con username y el total de cartas que tienen en su manos

# Acciones del juego

- [ ] Validar username

- [ ] El primer turno es al azar

- [ ] Cambiar de turno, teninendo en cuenta si se salta a un jugador o varios
- [ ] Perder turno / Saltar turno
- [ ] En cada turno conocemos la última carta en el mazo de descarte (símbolo y color), el jugador que tiene el turno y el tiempo restante del turno

- [ ] Reiniciar temporizador
- [ ] Detectar si el temporizador ha finalizado

- [ ] Cambiar de color

- [ ] El turno de un jugador finaliza cuando ha hecho una jugada, ha robado o se ha acabado el turno
- [ ] Si se acaba el turno, roba automáticamente 2 cartas y pierde el turno

- [x] Añadir carta a mazo de robo
- [x] Generar el mazo de robo
- [x] Mezclar cartas
- [x] Robar una carta consiste en coger la última carta del mazo de robo y devolver dicha carta
- [ ] Añadir carta a mazo de descarte
- [ ] Convertir mazo de descarte a mazo de robo. Se cambian todas las cartas de mazo salvo la última del mazo de descarte (la última jugada)

- [ ] Iniciar mano del jugador. Los jugadores empiezan con 7 cartas
- [ ] Añadir carta a la mano del jugador
- [ ] Eliminar carta de la mano del jugador

- [ ] Jugar una carta. En tu turno puedes jugar una o varias cartas (si son exactamente iguales en símbolo y color)
- [ ] Si el jugador es IA, utilizar corrutinas para que la jugada no sea immediata
- [ ] El jugador selecciona la carta o cartas a tirar con click izquierdo
- [ ] Validar si una carta se puede jugar. Se puede jugar una carta por símbolo (número o carta especial) o color
- [ ] Seleccionar una carta de la mano del jugador
- [ ] Si una carta está seleccionada y se le da un nuevo click izquierdo, vuelve a su estado original
- [ ] Si la carta seleccionada para la jugada es una carta válida, se agranda en la mano para mostrar visualmente que se puede jugar y está seleccionada para ser jugada
- [ ] Si la carta seleccionada no es válida para la jugada (teniendo en cuenta las cartas previamente seleccionadas), vuelve a su estado original y suena un SFX

- [ ] El jugador confirma la jugada con un botón adicional en la UI
- [ ] Si se hace click sobre el botón de confirmación de la jugada, se juegan todas las cartas seleccionadas

- [ ] En caso de no poder tirar una carta, se roba hasta tener una válida y es obligatorio jugarla
- [ ] El jugador solamente puede robar si no tiene ninguna carta que poder jugar
- [ ] Si el jugador quiere robar teniendo al menos una carta que poder jugar, sonará un SFX y se destacarán las cartas que se pueden jugar

- [ ] En caso de que a un jugador le quede una sola carta, aparecerá un botón en un punto aleatorio de la pantalla, sonará un SFX y el jugador con una carta tiene que ser el primero en darle al botón. Si otro jugador es más rápido en darle al botón, el jugador al que le quedaba una carta deberá robar dos cartas y pierde el turno

- [ ] En el mazo de descarte se pueden acumular solamente PLUS2 infinitamente. Es decir, después de un PLUS2 el siguiente jugador puede tirar otro PLUS2 y librarse de robar, pero no puede tirar un PLUS4
- [ ] Si al jugador le tiran un PLUS2 y dispone de un PLUS2, tiene que tirarlo, no puede guardárselo 
- [ ] En el mazo de descarte se pueden acumular solamente PLUS4 infinitamente. Es decir, después de un PLUS4 el siguiente jugador puede tirar otro PLUS4 y librarse de robar, pero no puede tirar un PLUS2
- [ ] Si al jugador le tiran un PLUS4 y dispone de un PLUS4, tiene que tirarlo, no puede guardárselo

- [ ] Si se salta un turno a un jugador por una carta SKIP, dicho jugador tendrá un icono de prohíbido sobre su icono de jugador. Si se trata del propio jugador el que está bloqueado, el botón de confirmar tendrá un icono de prohibido o estará deshabilitado 

- [ ] Configuar el volumen de la música
- [ ] Configuar el volumen de los SFX
- [ ] Configurar el tamaño de la ventana de juego
- [ ] Configurar el idioma
- [ ] Configurar los colores del mazo con opciones preconfiguradas (clásica y daltónicos)
- [ ] Configurar la duración del temporizador con opciones preconfiguradas
- [ ] Configurar la cantidad de mazos con opciones preconfiguradas (1 o 2)