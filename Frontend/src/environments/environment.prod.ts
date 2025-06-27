export const environment = {
  production: true,
    api_url: 'http://' + window.location.host + '/api',
    api_v2_url: 'http://' + window.location.host + '/apiV2',
    local_url: 'http://' + window.location.host + '/juridico/',
    s1_url: 'http://' + window.location.host + '/juridico',
    s2_url: `http://${window.location.host}/${window.location.host.includes('interno') ? 'trabalhista' : 'modulo-trabalhista'}` ,
  client_id: '4e7249eef1a946d1b6f6059031ab8ba9'
};
