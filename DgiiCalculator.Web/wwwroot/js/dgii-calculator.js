/* wwwroot/js/dgii-calculator.js */

let errorTimeout;

// FUNCIÓN PRINCIPAL DE ERROR VISUAL
function triggerError(message) {
    const input = document.getElementById('input-monto');
    const errorSpan = document.getElementById('js-error-message');

   
    input.classList.remove('input-error-shake');
    void input.offsetWidth; 
    input.classList.add('input-error-shake');

   
    errorSpan.innerText = message;
    errorSpan.classList.add('visible');

    clearTimeout(errorTimeout);
    errorTimeout = setTimeout(() => {
        input.classList.remove('input-error-shake');
        errorSpan.classList.remove('visible');
    }, 2000);
}

function validateAndSubmit() {
    const input = document.getElementById('input-monto');

    let val = input.value.trim().replace(/,/g, '');

    if (!val || val === '' || val === '.' || parseFloat(val) === 0) {
        triggerError("⚠️ El ingreso es obligatorio.");
        input.focus();
        return;
    }

    document.getElementById('calculator-form').submit();
}

function validateKeyPress(event) {
    const key = event.key;

    // Permitir teclas de control
    if (['Backspace', 'Delete', 'Tab', 'Enter', 'ArrowLeft', 'ArrowRight', 'Home', 'End'].includes(key) ||
        (event.ctrlKey === true) || (event.metaKey === true)) {

        if (key === 'Enter') {
            event.preventDefault(); 
            validateAndSubmit(); 
        }
        return;
    }

    //Signo Negativo
    if (key === '-' || key === 'Subtract') {
        event.preventDefault();
        triggerError("⚠️ No se permiten ingresos negativos."); 
        return;
    }

    //Letras o Símbolos
    if (!/^[0-9.]$/.test(key)) {
        event.preventDefault();
        triggerError("🚫 Solo se permiten números válidos."); 
        return;
    }

    //Múltiples Puntos
    if (key === '.') {
        const input = event.target;
        if (input.value.includes('.')) {
            event.preventDefault();
            triggerError("ℹ️ Ya existe un punto decimal.");
        }
    }
}

function formatAndValidate(input) {
    let value = input.value;

    // Limpieza profunda si pegan texto
    let cleanValue = value.replace(/[^0-9.]/g, '');

    // Validar estructura de puntos
    const parts = cleanValue.split('.');
    if (parts.length > 2) {
        cleanValue = parts[0] + '.' + parts.slice(1).join('');
    }

    // Limitar decimales
    if (parts.length > 1) {
        if (parts[1].length > 2) {
            cleanValue = parts[0] + '.' + parts[1].slice(0, 2);
        }
    }

    // Limitar longitud
    if (cleanValue.length > 15) {
        cleanValue = cleanValue.slice(0, 15);
    }

    if (value !== cleanValue) {
        input.value = cleanValue;
        triggerError("Formato ajustado automáticamente.");
    }
}

function finalizeInput(input) {
    if (input.value === '.') {
        input.value = '';
    } else if (input.value.startsWith('.')) {
        input.value = '0' + input.value;
    }
}

function toggleAccordion(id) {
    const allItems = document.querySelectorAll('.acc-item');
    const targetItem = document.getElementById(id);
    const isAlreadyActive = targetItem.classList.contains('active');
    allItems.forEach(item => item.classList.remove('active'));
    if (!isAlreadyActive) targetItem.classList.add('active');
}