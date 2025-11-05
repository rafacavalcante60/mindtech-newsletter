/**
 * @jest-environment jsdom
 */

const fs = require('fs');
const path = require('path');
const fetchMock = require('jest-fetch-mock');
const { fireEvent, screen, waitFor } = require('@testing-library/dom');

fetchMock.enableMocks();

let container;

beforeEach(() => {
    fetch.resetMocks();

    const html = fs.readFileSync(path.resolve(__dirname, '../src/index.html'), 'utf8');
    document.body.innerHTML = html;
    container = document.body;

    require('../src/script.js');
});

afterEach(() => {
    jest.resetModules();
});

describe('Newsletter Subscribe', () => {
    test('mostra erro se email vazio', async () => {
        const form = container.querySelector('#subscribeForm');
        const emailInput = container.querySelector('#email');
        const errorMessage = container.querySelector('#errorMessage');

        emailInput.value = '';
        fireEvent.submit(form);

        await waitFor(() => {
            expect(errorMessage.textContent).toBe('Por favor, insira um e-mail válido.');
        });
    });

    test('mostra erro se email inválido', async () => {
        const form = container.querySelector('#subscribeForm');
        const emailInput = container.querySelector('#email');
        const errorMessage = container.querySelector('#errorMessage');

        emailInput.value = 'email-invalido';
        fireEvent.submit(form);

        await waitFor(() => {
            expect(errorMessage.textContent).toBe('Por favor, insira um e-mail válido.');
        });
    });

    test('chama API ao enviar email válido e mostra sucesso', async () => {
        fetch.mockResponseOnce(JSON.stringify({ success: true }), { status: 200 });

        const form = container.querySelector('#subscribeForm');
        const emailInput = container.querySelector('#email');
        const subscribeScreen = container.querySelector('#subscribeScreen');
        const successScreen = container.querySelector('#successScreen');

        emailInput.value = 'teste@mindtech.com';
        fireEvent.submit(form);

        await waitFor(() => {
            expect(subscribeScreen.classList.contains('hidden')).toBe(true);
            expect(successScreen.classList.contains('hidden')).toBe(false);
        });
    });

    test('mostra erro se email já cadastrado (409)', async () => {
        fetch.mockResponseOnce(JSON.stringify({ message: 'já cadastrado' }), { status: 409 });

        const form = container.querySelector('#subscribeForm');
        const emailInput = container.querySelector('#email');
        const errorMessage = container.querySelector('#errorMessage');

        emailInput.value = 'teste@mindtech.com';
        fireEvent.submit(form);

        await waitFor(() => {
            expect(errorMessage.textContent).toBe('Este e-mail já está cadastrado na nossa newsletter.');
        });
    });
});

describe('Newsletter Unsubscribe', () => {
    test('abre e fecha modal corretamente', () => {
        const unsubscribeBtn = container.querySelector('#unsubscribeBtn');
        const modal = container.querySelector('#unsubscribeModal');
        const closeBtn = container.querySelector('#modalClose');

        fireEvent.click(unsubscribeBtn);
        expect(modal.classList.contains('show')).toBe(true);

        fireEvent.click(closeBtn);
        expect(modal.classList.contains('show')).toBe(false);
    });

    test('mostra erro se email inválido no descadastro', async () => {
        const unsubscribeBtn = container.querySelector('#unsubscribeBtn');
        const confirmBtn = container.querySelector('#confirmUnsubscribeBtn');
        const emailInput = container.querySelector('#unsubscribeEmail');
        const errorElement = container.querySelector('#unsubscribeError');

        fireEvent.click(unsubscribeBtn);

        emailInput.value = 'invalido';
        fireEvent.click(confirmBtn);

        await waitFor(() => {
            expect(errorElement.textContent).toBe('Por favor, insira um e-mail válido.');
        });
    });

    test('chama API de descadastro e mostra notification', async () => {
        fetch.mockResponseOnce(JSON.stringify({ success: true }), { status: 200 });

        const unsubscribeBtn = container.querySelector('#unsubscribeBtn');
        const confirmBtn = container.querySelector('#confirmUnsubscribeBtn');
        const emailInput = container.querySelector('#unsubscribeEmail');
        const notification = container.querySelector('#notification');

        fireEvent.click(unsubscribeBtn);
        emailInput.value = 'teste@mindtech.com';
        fireEvent.click(confirmBtn);

        await waitFor(() => {
            expect(notification.textContent).toBe('E-mail removido da newsletter com sucesso!');
        });
    });

    test('mostra erro se email não cadastrado (404)', async () => {
        fetch.mockResponseOnce(JSON.stringify({ message: 'não encontrado' }), { status: 404 });

        const unsubscribeBtn = container.querySelector('#unsubscribeBtn');
        const confirmBtn = container.querySelector('#confirmUnsubscribeBtn');
        const emailInput = container.querySelector('#unsubscribeEmail');
        const errorElement = container.querySelector('#unsubscribeError');

        fireEvent.click(unsubscribeBtn);
        emailInput.value = 'naoexiste@mindtech.com';
        fireEvent.click(confirmBtn);

        await waitFor(() => {
            expect(errorElement.textContent).toBe('Este e-mail não está cadastrado na nossa newsletter.');
        });
    });
});
