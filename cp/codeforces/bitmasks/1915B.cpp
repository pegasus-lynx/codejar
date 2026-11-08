// 1915B - Not Quite Latin Square
// Bitmask, freq count

#include<bits/stdc++.h>
using namespace std;

void solve(){
	char a = 'A', b = 'B', c = 'C', q = '?';
	int x = (int)a ^ (int)b ^ (int)c ^ (int)q;
	int r = 3;
	while(r--)
	{
		cin >> a >> b >> c;
		x ^= (int)a;
		x ^= (int)b;
		x ^= (int)c;
	}

	cout << (char)x << "\n";
}

signed main(){
	ios_base::sync_with_stdio(0);
	cin.tie(0);cout.tie(0);
	int t; cin >> t; while(t--)
	solve();
	return 0;
}