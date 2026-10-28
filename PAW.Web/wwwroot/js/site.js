// Write your JavaScript code.

const EXCHANGE_RATE_URL = 'https://api.hacienda.go.cr/indicadores/tc';
const EXCHANGE_RATE_CACHE_KEY = 'paw.exchangeRate';
const EXCHANGE_RATE_COLLAPSED_KEY = 'paw.exchangeRate.collapsed';
const EXCHANGE_RATE_CACHE_MS = 60 * 60 * 1000; // 1 hora

let exchangeRatePending = null; // evita llamadas duplicadas mientras una está en curso

function readExchangeRateCache() {
    try {
        const raw = localStorage.getItem(EXCHANGE_RATE_CACHE_KEY);
        return raw ? JSON.parse(raw) : null;
    } catch {
        return null;
    }
}

function saveExchangeRateCache(data) {
    try {
        localStorage.setItem(EXCHANGE_RATE_CACHE_KEY, JSON.stringify({ savedAt: Date.now(), data }));
    } catch { /* sin cache, pero sigue funcionando */ }
}

// Devuelve { data, fromCache, stale? }
async function getExchangeRate() {
    const cached = readExchangeRateCache();
    if (cached && Date.now() - cached.savedAt < EXCHANGE_RATE_CACHE_MS) {
        return { data: cached.data, fromCache: true };
    }

    if (!exchangeRatePending) {
        exchangeRatePending = fetch(EXCHANGE_RATE_URL, { headers: { Accept: 'application/json' } })
            .then(response => {
                if (!response.ok) throw new Error('Exchange rate API answered HTTP ' + response.status);
                return response.json();
            })
            .then(data => {
                saveExchangeRateCache(data);
                return { data, fromCache: false };
            })
            .finally(() => { exchangeRatePending = null; });
    }

    try {
        return await exchangeRatePending;
    } catch (error) {
        if (cached) return { data: cached.data, fromCache: true, stale: true };
        throw error;
    }
}

function formatRateMoney(symbol, value, digits = 2) {
    return symbol + Number(value).toLocaleString('en-US', {
        minimumFractionDigits: digits,
        maximumFractionDigits: digits
    });
}

function formatRateDate(value) {
    const [year, month, day] = String(value).substring(0, 10).split('-').map(Number);
    if (!year || !month || !day) return String(value);
    return new Date(year, month - 1, day)
        .toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
}

async function loadExchangeRate() {
    const widget = document.getElementById('exchange-rate');
    if (!widget) return;

    const set = (name, text) => {
        const element = widget.querySelector('[data-rate="' + name + '"]');
        if (element) element.textContent = text;
    };

    widget.dataset.state = 'loading';

    try {
        const { data, fromCache, stale } = await getExchangeRate();
        const colon = '\u20A1';

        set('usd-buy', formatRateMoney(colon, data.dolar.compra.valor));
        set('usd-sell', formatRateMoney(colon, data.dolar.venta.valor));
        set('eur-crc', formatRateMoney(colon, data.euro.colones));
        set('eur-usd', formatRateMoney('$', data.euro.dolares, 4));
        set('updated', 'Updated ' + formatRateDate(data.dolar.venta.fecha)
            + (stale ? ' \u00B7 offline copy' : fromCache ? ' \u00B7 cached' : ''));

        widget.dataset.state = stale ? 'stale' : 'ready';
    } catch (error) {
        console.error('The exchange rate could not be loaded.', error);
        set('updated', 'Exchange rate unavailable');
        widget.dataset.state = 'error';
    }
}

function initExchangeRateWidget() {
    const widget = document.getElementById('exchange-rate');
    const toggle = document.getElementById('exchange-rate-toggle');
    if (!widget || !toggle) return;

    const setCollapsed = collapsed => {
        widget.classList.toggle('is-collapsed', collapsed);
        toggle.setAttribute('aria-expanded', String(!collapsed));
    };

    try {
        setCollapsed(localStorage.getItem(EXCHANGE_RATE_COLLAPSED_KEY) === '1');
    } catch { }

    toggle.addEventListener('click', function() {
        const collapsed = !widget.classList.contains('is-collapsed');
        setCollapsed(collapsed);
        try { localStorage.setItem(EXCHANGE_RATE_COLLAPSED_KEY, collapsed ? '1' : '0'); } catch { }
    });

    loadExchangeRate();
}

if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initExchangeRateWidget);
} else {
    initExchangeRateWidget();
}