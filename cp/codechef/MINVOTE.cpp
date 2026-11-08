#include<bits/stdc++.h> 
using namespace std;

#define int         long long
#define ull         unsigned long long
#define ll          long long
#define pb          push_back
#define all(a)      (a).begin(),(a).end()
#define sz(x)       (ll)x.size()
#define endl        '\n'
#define gcd(a,b)    __gcd((a),(b))
#define lcm(a,b)    ((a)*(b)) / gcd((a),(b))
#define ios         ios_base::sync_with_stdio(false);cin.tie(0);cout.tie(0);
#define mp          make_pair
#define lb          lower_bound
#define ub          upper_bound
#define F           first
#define S           second
#define rep(i, begin, end) for (__typeof(end) i = (begin) - ((begin) > (end)); i != (end) - ((begin) > (end)); i += 1 - 2 * ((begin) > (end)))
#define hell        (ull)1e9

void solve();

signed main(void)
{
    ios
    freopen("input.txt", "r", stdin);freopen("output.txt", "w", stdout);
    int TESTS=1;
    cin>>TESTS;

    while(TESTS--){
        solve();
    }
    return 0;
}

void solve()
{
    int n;
    cin >> n;
    vector<int> s(n,0);

    vector<int> pre(n,0);
    vector<int> suf(n,0);

    rep(i,0,n) cin >> s[i];
    // pre[0] = s[0];
    // suf[n-1] = s[n-1];
    rep(i,1,n)
    {
        pre[i] = pre[i-1]+s[i-1];
        suf[(n-1)-i] = suf[n-i] + s[n-i];
    }

    // rep(i,0,n) cout << pre[i] << " ";
    // cout << "\n";

    vector<int> votes(n,0);

    rep(i,0,n-1)
    {
        auto it = upper_bound(all(pre), s[i]+pre[i+1]);
        // cout << i << " " << it-pre.begin() << "\n";
        rep(j,i+1,it-pre.begin()) votes[j]++;
    }

    // rep(i,0,n) cout << suf[i] << " ";
    // cout << "\n";

    reverse(all(suf));

    rep(i,0,n-1)
    {
        auto it = upper_bound(all(suf), s[(n-1)-i]+suf[i+1]);
        // cout << i << " " << it-suf.begin() << "\n";
        rep(j,i+1,it-suf.begin()) votes[(n-1)-j]++;
    }

    rep(i,0,n) cout << votes[i] << " ";
    cout << "\n";

}


