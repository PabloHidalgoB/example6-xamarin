# Aplicación de ejemplo 6 - Xamarin

Esta aplicacion es en su core, la misma que la aplicacion de ejemplo 6 de android, rescata un objeto JSON desde la siguiente pagina de 
internet http://www.floatrates.com/daily/ils.json y la separa para ser desplegado en los spinner de seleccion de monedas, al igual que sus 
valores de cambio como se puede apreciar en la siguiente imagen

<img src="https://github.com/PabloHidalgoB/example6-xamarin/blob/master/example6-xamarin/example6/Resources/drawable/screenshots/main.png" data-canonical-src="https://github.com/PabloHidalgoB/example6-xamarin/blob/master/example6-xamarin/example6/Resources/drawable/screenshots/main.png" width="200" height="auto" />

La diferencia que esta aplicacion posee con el ejemplo de Android radica en que este proyecto trabaja con un codigo base que puede ser
compartida con las plataformas de Windows, Android, iOS y Linux si se decide por tomar esas rutas al igual que trabaja con un fragment que 
es el encargado de controlar la parte conversora de la aplicacion

Dicho eso, la funcionalidad del programa sigue siendo la misma, la moneda de entrada seleccionada de manera predeterminada para el cambio 
inicial es el Peso chileno, el boton "Cambiar valores" cambia de posicion entre los spinners para realizar una conversion entre una moneda 
a otra

<img src="https://github.com/PabloHidalgoB/example6-xamarin/blob/master/example6-xamarin/example6/Resources/drawable/screenshots/switch.png" data-canonical-src="https://github.com/PabloHidalgoB/example6-xamarin/blob/master/example6-xamarin/example6/Resources/drawable/screenshots/switch.png" width="200" height="auto" />

De la misma forma este programa muestra una vista rapida del valor de las 5 monedas mas utilizadas en el mundo expresadas en pesos 
chilenos

<img src="https://github.com/PabloHidalgoB/example6-xamarin/blob/master/example6-xamarin/example6/Resources/drawable/screenshots/scroll.png" data-canonical-src="https://github.com/PabloHidalgoB/example6-xamarin/blob/master/example6-xamarin/example6/Resources/drawable/screenshots/scroll.png" width="200" height="auto" />

Cabe destacar que tanto el teclado como el boton convertir son capazes de realizar la misma operacion el cual es realizar la conversion
de las monedas seleccionadas y desplegar el resultado en la mitad de la pantalla

<img src="https://github.com/PabloHidalgoB/example6-xamarin/blob/master/example6-xamarin/example6/Resources/drawable/screenshots/conversion.png" data-canonical-src="https://github.com/PabloHidalgoB/example6-xamarin/blob/master/example6-xamarin/example6/Resources/drawable/screenshots/conversion.png" width="200" height="auto" />

Se debe mencionar que una conexion a internet es requerida para el funcionamiento del programa, de lo contrario este no puede obtener
y desplegar los datos o hacer uso de operacion alguna que el usuario pueda requerir
