/* wwwroot/js/dgii-calculator.js */

// Función para el Acordeón
function toggleAccordion(id) {
    const allItems = document.querySelectorAll('.acc-item');
    const targetItem = document.getElementById(id);
    const isAlreadyActive = targetItem.classList.contains('active');

    allItems.forEach(item => {
        item.classList.remove('active');
    });

    if (!isAlreadyActive) {
        targetItem.classList.add('active');
    }
}

//Validación en tiempo real del dinero
function validateMoneyInput(input) {
    let value = input.value;

    //Si empieza con punto, agregar 0 antes (Ej: .88 -> 0.88)
    if (value.startsWith('.')) {
        value = '0' + value;
    }

    // Limitar a 2 decimales
    if (value.includes('.')) {
        let parts = value.split('.');
        // Si la parte decimal tiene más de 2 dígitos, cortarla
        if (parts[1].length > 2) {
            parts[1] = parts[1].slice(0, 2);
            value = parts[0] + '.' + parts[1];
        }
    }

    // Limitar longitud total (Ej: 15 caracteres)
    if (value.length > 15) {
        value = value.slice(0, 15);
    }

    // Actualizar el valor en el campo
    if (input.value !== value) {
        input.value = value;
    }
}

//Hacer foco en el campo numérico
function focusAmount() {
    // Buscamos el input por su ID generado por ASP.NET (Input_IncomeAmount)
    const input = document.getElementById('Input_IncomeAmount');
    if (input) {
        input.focus();
        //Seleccionar todo el texto al recibir foco para facilitar edición
       input.select(); 
    }
}

//Detectar tecla ENTER para calcular
function checkEnter(event) {
    if (event.key === "Enter") {
        event.preventDefault();
        document.querySelector('.btn-dgii-action').click();
    }
}

//Autofocus al cargar la página
window.addEventListener('DOMContentLoaded', (event) => {
    focusAmount();
});