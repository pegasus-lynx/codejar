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
int bsstring(int start, int end, int size);

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
    int n,m;
    cin >> n >> m;

    vector<vector<int>> a(n,vector<int>(m,0)); 
    vector<vector<int>> b(n,vector<int>(m,0)); 
    vector<int> even_a,odd_a;
    vector<int> even_b,odd_b;
    rep(i,0,n)
    {
        rep(j,0,m)
        {
            cin >> a[i][j];
            if((i+j)%2)
            {
                odd_a.pb(a[i][j]);
            }
            else
            {
                even_a.pb(a[i][j]);
            }
        }
    }

    rep(i,0,n)
    {
        rep(j,0,m)
        {
            cin >> b[i][j];
            if((i+j)%2)
            {
                odd_b.pb(b[i][j]);
            }
            else
            {
                even_b.pb(b[i][j]);
            }
        }
    }

    if(n!=1 && m!=1)
    {
        sort(all(odd_a));
        sort(all(odd_b));
    }
    if(sz(odd_a) != sz(odd_b))
    {
        cout << "NO\n";
        return;
    }

    int size = sz(odd_a);
    rep(i,0,size)
    {
        if(odd_a[i]!=odd_b[i])
        {
            cout << "NO\n";
            return;
        }
    }

    cout << "YES\n";
    return;
}

