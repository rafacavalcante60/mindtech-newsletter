const API_BASE_URL = "https://localhost:7035/api/subscriber";

const subscribeForm = document.getElementById('subscribeForm');
const emailInput = document.getElementById('email');
const errorMessage = document.getElementById('errorMessage');
const submitBtn = document.getElementById('submitBtn');
const subscribeScreen = document.getElementById('subscribeScreen');
const successScreen = document.getElementById('successScreen');
const notification = document.getElementById('notification');

// Elementos do modal
const unsubscribeBtn = document.getElementById('unsubscribeBtn');
const unsubscribeModal = document.getElementById('unsubscribeModal');
const modalClose = document.getElementById('modalClose');
const cancelBtn = document.getElementById('cancelBtn');
const unsubscribeEmail = document.getElementById('unsubscribeEmail');
const unsubscribeError = document.getElementById('unsubscribeError');
const confirmUnsubscribeBtn = document.getElementById('confirmUnsubscribeBtn');

function showError(message, errorElement, inputElement) {
    errorElement.textContent = message;
    errorElement.classList.add('show');
    inputElement.classList.add('error');
}

function hideError(errorElement, inputElement) {
    errorElement.classList.remove('show');
    inputElement.classList.remove('error');
}

function showNotification(message, isError = false) {
    notification.textContent = message;
    notification.style.background = isError ? '#ff3b30' : '#34c759';
    notification.classList.add('show');
    
    setTimeout(() => {
        notification.classList.remove('show');
    }, 4000);
}

function showSuccessScreen() {
    subscribeScreen.classList.add('hidden');
    successScreen.classList.remove('hidden');
}

function openUnsubscribeModal() {
    unsubscribeModal.classList.add('show');
    unsubscribeEmail.value = '';
    hideError(unsubscribeError, unsubscribeEmail);
}

function closeUnsubscribeModal() {
    unsubscribeModal.classList.remove('show');
}

//event listeners para o modal
unsubscribeBtn.addEventListener('click', openUnsubscribeModal);
modalClose.addEventListener('click', closeUnsubscribeModal);
cancelBtn.addEventListener('click', closeUnsubscribeModal);

//close modal by clicking outside
unsubscribeModal.addEventListener('click', (e) => {
    if (e.target === unsubscribeModal) {
        closeUnsubscribeModal();
    }
});

emailInput.addEventListener('input', () => {
    hideError(errorMessage, emailInput);
});

unsubscribeEmail.addEventListener('input', () => {
    hideError(unsubscribeError, unsubscribeEmail);
});

//subscribe form
subscribeForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    hideError(errorMessage, emailInput);

    const email = emailInput.value.trim();

    if (!email) {
        showError('Por favor, insira um e-mail válido.', errorMessage, emailInput);
        return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        showError('Por favor, insira um e-mail válido.', errorMessage, emailInput);
        return;
    }

    submitBtn.disabled = true;
    submitBtn.textContent = 'Inscrevendo...';

    try {
        const response = await fetch(`${API_BASE_URL}`, { // só API_BASE_URL
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email: email })
        });

        const data = await response.json();

        if (response.ok) {
            showSuccessScreen();
            emailInput.value = '';
        } else {
            if (response.status === 409 || data.message?.includes('já cadastrado')) {
                showError('Este e-mail já está cadastrado na nossa newsletter.', errorMessage, emailInput);
            } else {
                showError(data.message || 'Erro ao realizar inscrição. Tente novamente.', errorMessage, emailInput);
            }
        }
    } catch (error) {
        console.error('Erro:', error);
        showError('Erro ao conectar com o servidor. Verifique se a API está rodando.', errorMessage, emailInput);
    } finally {
        submitBtn.disabled = false;
        submitBtn.textContent = 'Inscrever-se';
    }
});

//unsubscribe functionality
confirmUnsubscribeBtn.addEventListener('click', async () => {
    hideError(unsubscribeError, unsubscribeEmail);

    const email = unsubscribeEmail.value.trim();

    if (!email) {
        showError('Por favor, insira um e-mail válido.', unsubscribeError, unsubscribeEmail);
        return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        showError('Por favor, insira um e-mail válido.', unsubscribeError, unsubscribeEmail);
        return;
    }

    confirmUnsubscribeBtn.disabled = true;
    confirmUnsubscribeBtn.textContent = 'Descadastrando...';

    try {
        const response = await fetch(`${API_BASE_URL}/unsubscribe`, { // endpoint correto
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email: email })
        });

        const data = await response.json();

        if (response.ok) {
            closeUnsubscribeModal();
            showNotification('E-mail removido da newsletter com sucesso!');
            unsubscribeEmail.value = '';
        } else {
            if (response.status === 404 || data.message?.includes('não encontrado')) {
                showError('Este e-mail não está cadastrado na nossa newsletter.', unsubscribeError, unsubscribeEmail);
            } else {
                showError(data.message || 'Erro ao realizar descadastro. Tente novamente.', unsubscribeError, unsubscribeEmail);
            }
        }
    } catch (error) {
        console.error('Erro:', error);
        showError('Erro ao conectar com o servidor. Verifique se a API está rodando.', unsubscribeError, unsubscribeEmail);
    } finally {
        confirmUnsubscribeBtn.disabled = false;
        confirmUnsubscribeBtn.textContent = 'Descadastrar';
    }
});

//permite voltar para a tela de inscrição (para testes)
successScreen.addEventListener('click', () => {
    successScreen.classList.add('hidden');
    subscribeScreen.classList.remove('hidden');
});
